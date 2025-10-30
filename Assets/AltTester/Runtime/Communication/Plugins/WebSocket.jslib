var LibraryAltTesterWebSocket = {
	$altTesterWebSocketState: {
		/*
		 * Map of instances
		 *
		 * Instance structure:
		 * {
		 * 	url: string,
		 * 	ws: WebSocket
		 * }
		 */
		instances: {},

		/* Last instance ID */
		lastId: 0,

		/* Event listeners */
		onOpen: null,
		onMessage: null,
		onError: null,
		onClose: null,

		/* Debug mode */
		debug: false
	},

	/**
	 * Set onOpen callback
	 *
	 * @param callback Reference to C# static function
	 */
	AltTesterWebSocketSetOnOpen: function (callback) {
		altTesterWebSocketState.onOpen = callback;
	},

	/**
	 * Set onMessage callback
	 *
	 * @param callback Reference to C# static function
	 */
	AltTesterWebSocketSetOnMessage: function (callback) {
		altTesterWebSocketState.onMessage = callback;
	},

	/**
	 * Set onError callback
	 *
	 * @param callback Reference to C# static function
	 */
	AltTesterWebSocketSetOnError: function (callback) {
		altTesterWebSocketState.onError = callback;
	},

	/**
	 * Set onClose callback
	 *
	 * @param callback Reference to C# static function
	 */
	AltTesterWebSocketSetOnClose: function (callback) {
		altTesterWebSocketState.onClose = callback;
	},

	/**
	 * Allocate new WebSocket instance struct
	 *
	 * @param url Server URL
	 */
	AltTesterWebSocketAllocate: function (url) {
		var urlStr = UTF8ToString(url);
		var id = altTesterWebSocketState.lastId++;

		altTesterWebSocketState.instances[id] = {
			subprotocols: [],
			url: urlStr,
			ws: null
		};

		return id;
	},

	/**
	 * Add subprotocol to instance
	 *
	 * @param instanceId Instance ID
	 * @param subprotocol Subprotocol name to add to instance
	 */
	AltTesterWebSocketAddSubProtocol: function (instanceId, subprotocol) {
		var subprotocolStr = UTF8ToString(subprotocol);
		altTesterWebSocketState.instances[instanceId].subprotocols.push(subprotocolStr);
	},

	/**
	 * Remove reference to WebSocket instance
	 *
	 * If socket is not closed function will close it but onClose event will not be emitted because
	 * this function should be invoked by C# WebSocket destructor.
	 *
	 * @param instanceId Instance ID
	 */
	AltTesterWebSocketFree: function (instanceId) {
		var instance = altTesterWebSocketState.instances[instanceId];

		if (!instance) {
			return 0;
		}

		// Close if not closed
		if (instance.ws && instance.ws.readyState < 2) {
			try {
				instance.ws.close(code, reason);
			} catch
			{
			}
		}

		// Remove reference
		delete altTesterWebSocketState.instances[instanceId];

		return 0;
	},

	/**
	 * Connect WebSocket to the server
	 *
	 * @param instanceId Instance ID
	 */
	AltTesterWebSocketConnect: function (instanceId) {
		var instance = altTesterWebSocketState.instances[instanceId];
		if (!instance)
		{
			return -1;
		}

		if (instance.ws !== null) {
			return -2;
		}

		instance.ws = new WebSocket(instance.url, instance.subprotocols);

		instance.ws.binaryType = 'arraybuffer';

		instance.ws.onopen = function () {
			if (altTesterWebSocketState.debug) {
				console.log("[AltTester WebSocket] Connected.");
			}

			if (altTesterWebSocketState.onOpen) {
				// Unity 6+ uses different call method
				if (typeof dynCall_vi !== 'undefined') {
					dynCall_vi(altTesterWebSocketState.onOpen, instanceId);
				} else if (Module.dynCall_vi) {
					Module.dynCall_vi(altTesterWebSocketState.onOpen, instanceId);
				} else {
					// Fallback for newer Emscripten versions
					wasmTable.get(altTesterWebSocketState.onOpen)(instanceId);
				}
			}
		};

		instance.ws.onmessage = function (ev) {
			if (altTesterWebSocketState.debug) {
				console.log("[AltTester WebSocket] Received message:", ev.data);
			}

			if (altTesterWebSocketState.onMessage === null) {
				return;
			}

			if (ev.data instanceof ArrayBuffer) {
				var dataBuffer = new Uint8Array(ev.data);

				var buffer = _malloc(dataBuffer.length);
				HEAPU8.set(dataBuffer, buffer);

				try {
					// Unity 6+ compatibility
					if (typeof dynCall_viii !== 'undefined') {
						dynCall_viii(altTesterWebSocketState.onMessage, instanceId, buffer, dataBuffer.length);
					} else if (Module.dynCall_viii) {
						Module.dynCall_viii(altTesterWebSocketState.onMessage, instanceId, buffer, dataBuffer.length);
					} else {
						wasmTable.get(altTesterWebSocketState.onMessage)(instanceId, buffer, dataBuffer.length);
					}
				} finally {
					_free(buffer);
				}
			} else {
				var dataBuffer = (new TextEncoder()).encode(ev.data);

				var buffer = _malloc(dataBuffer.length);
				HEAPU8.set(dataBuffer, buffer);

				try {
					if (typeof dynCall_viii !== 'undefined') {
						dynCall_viii(altTesterWebSocketState.onMessage, instanceId, buffer, dataBuffer.length);
					} else if (Module.dynCall_viii) {
						Module.dynCall_viii(altTesterWebSocketState.onMessage, instanceId, buffer, dataBuffer.length);
					} else {
						wasmTable.get(altTesterWebSocketState.onMessage)(instanceId, buffer, dataBuffer.length);
					}
				} finally {
					_free(buffer);
				}
			}
		};

		instance.ws.onerror = function (ev) {
			if (altTesterWebSocketState.debug) {
				console.log("[AltTester WebSocket] Error occured.");
			}

			if (altTesterWebSocketState.onError) {
				var msg = "AltTester WebSocket error.";
				var length = lengthBytesUTF8(msg) + 1;
				var buffer = _malloc(length);
				stringToUTF8(msg, buffer, length);

				try {
					if (typeof dynCall_vii !== 'undefined') {
						dynCall_vii(altTesterWebSocketState.onError, instanceId, buffer);
					} else if (Module.dynCall_vii) {
						Module.dynCall_vii(altTesterWebSocketState.onError, instanceId, buffer);
					} else {
						wasmTable.get(altTesterWebSocketState.onError)(instanceId, buffer);
					}
				} finally {
					_free(buffer);
				}
			}
		};

		instance.ws.onclose = function (ev) {
			var reason = ev.reason;
			var length = lengthBytesUTF8(reason) + 1;
			var buffer = _malloc(length);
			stringToUTF8(reason, buffer, length);

			if (altTesterWebSocketState.debug) {
				console.log("[AltTester WebSocket] Closed.");
			}

			if (altTesterWebSocketState.onClose) {
				if (typeof dynCall_viii !== 'undefined') {
					dynCall_viii(altTesterWebSocketState.onClose, instanceId, ev.code, buffer);
				} else if (Module.dynCall_viii) {
					Module.dynCall_viii(altTesterWebSocketState.onClose, instanceId, ev.code, buffer);
				} else {
					wasmTable.get(altTesterWebSocketState.onClose)(instanceId, ev.code, buffer);
				}
			}

			delete instance.ws;
		};

		return 0;
	},

	/**
	 * Close WebSocket connection
	 *
	 * @param instanceId Instance ID
	 * @param code Close status code
	 * @param reasonPtr Pointer to reason string
	 */
	AltTesterWebSocketClose: function (instanceId, code, reasonPtr) {
		var instance = altTesterWebSocketState.instances[instanceId];
		if (!instance) {
			return -1;
		}

		if (!instance.ws) {
			return -3;
		}

		// Check if closing
		if (instance.ws.readyState === 2) {
			return -4;
		}

		// Check if closed
		if (instance.ws.readyState === 3) {
			return -5;
		}

		var reason = (reasonPtr ? UTF8ToString(reasonPtr) : undefined);

		try {
			instance.ws.close(code, reason);
		} catch (err) {
			return -7;
		}

		return 0;
	},

	/**
	 * Send message over WebSocket
	 *
	 * @param instanceId Instance ID
	 * @param bufferPtr Pointer to the message buffer
	 * @param length Length of the message in the buffer
	 */
	AltTesterWebSocketSend: function (instanceId, bufferPtr, length) {
		var instance = altTesterWebSocketState.instances[instanceId];
		if (!instance) {
			return -1;
		}

		if (!instance.ws) {
			return -3;
		}

		if (instance.ws.readyState !== 1) {
			return -6;
		}

		instance.ws.send(HEAPU8.buffer.slice(bufferPtr, bufferPtr + length));
		return 0;
	},

	/**
	 * Send text message over WebSocket
	 *
	 * @param instanceId Instance ID
	 * @param message Message string
	 */
	AltTesterWebSocketSendText: function (instanceId, message) {
		var instance = altTesterWebSocketState.instances[instanceId];
		if (!instance) {
			return -1;
		}

		if (!instance.ws) {
			return -3;
		}

		if (instance.ws.readyState !== 1) {
			return -6;
		}

		instance.ws.send(UTF8ToString(message));

		return 0;
	},

	/**
	 * Return WebSocket readyState
	 *
	 * @param instanceId Instance ID
	 */
	AltTesterWebSocketGetState: function (instanceId) {

		var instance = altTesterWebSocketState.instances[instanceId];
		if (!instance) return -1;

		if (instance.ws) {
			return instance.ws.readyState;
		} else {
			return 3; // Closed.
		}
	}

};

autoAddDeps(LibraryAltTesterWebSocket, '$altTesterWebSocketState');
mergeInto(LibraryManager.library, LibraryAltTesterWebSocket);

// https://github.com/WalletConnect/WalletConnectUnity/pull/36 - solved runtime/module error
// https://github.com/emscripten-core/emscripten/pull/8011 - solved UTF8 encoding error

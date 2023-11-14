package com.alttester.utils;

import java.util.Iterator;
import java.util.List;
import java.net.InetSocketAddress;
import java.net.Proxy;
import java.net.ProxySelector;
import java.net.URI;

public class AltProxyFinder {
    public static String getProxy(String uri) {
        try {
			System.setProperty("java.net.useSystemProxies", "true");

            List<Proxy> proxies = ProxySelector.getDefault().select(new URI(uri));

            for (Iterator<Proxy> iter = proxies.iterator(); iter.hasNext(); ) {
                Proxy proxy = iter.next();
                InetSocketAddress address = (InetSocketAddress) proxy.address();

                if (address != null) {
                    return String.format("%s://%s:%s", proxy.type(), address.getHostName(), address.getPort());
                }
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

		return null;
    }
}

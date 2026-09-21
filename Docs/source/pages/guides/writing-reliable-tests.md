# Writing reliable tests

Game UIs are animated, frame-dependent and asynchronous. A button exists before it is interactive;
a menu is visible while it is still sliding in; a scene reports as loaded before its objects are
wired up. Most flaky AltTester® tests come from acting on the app before it is ready, and the fix
is almost always choosing the right wait rather than adding a sleep.

This page explains the timeouts AltTester® gives you, how they interact, and which one to reach
for.

## Four different timeouts

They are easy to confuse because three of them are called "timeout". They govern different things
and fail with different exceptions.

```eval_rst
.. list-table::
   :widths: 26 12 14 48
   :header-rows: 1

   * - Setting
     - Default
     - Unit
     - Governs
   * - ``AltDriver(timeout=…)``
     - ``60``
     - seconds
     - How long the driver waits to establish its connection to the AltTester® Server, once, at
       construction. Set it to ``None`` to wait indefinitely. Fails with
       ``ConnectionTimeoutError``.
   * - Command response timeout
     - see below
     - seconds
     - How long the driver waits for the app to answer **any single command**. Set with
       ``SetCommandResponseTimeout``. Fails with ``CommandResponseTimeoutException``.
   * - ``timeout`` on a ``WaitFor…``
     - ``20``
     - seconds
     - How long that one call keeps re-checking its condition. Fails with
       ``WaitTimeOutException``.
   * - Implicit timeout
     - off
     - seconds
     - A default applied to ``WaitFor…`` calls that did not set their own. Set with
       ``SetImplicitTimeout``. See the caveat below.
```

The distinction that matters most in practice:

- **`WaitTimeOutException`** — the app is alive and answering, but the thing you waited for did
  not happen. Look at your test, your selector, or the game state.
- **`CommandResponseTimeoutException`** — the app did not answer at all. Look at the app: it is
  busy, frozen, paused, minimised, or gone.

Raising a wait timeout will never fix a `CommandResponseTimeoutException`, and vice versa.

## How a wait actually works

A `WaitFor…` call polls. It runs its underlying command, and if the condition is not met it sleeps
for `interval` and tries again, until `timeout` elapses.

```eval_rst
.. list-table::
   :widths: 24 14 62
   :header-rows: 1

   * - Parameter
     - Default
     - Meaning
   * - ``timeout``
     - ``20``
     - Total seconds to keep trying before raising ``WaitTimeOutException``.
   * - ``interval``
     - ``0.5``
     - Seconds between attempts. Must be smaller than ``timeout``.
```

Two consequences worth internalising:

- **Each attempt is a full round trip.** A short `interval` against a slow app does not make the
  wait more responsive; it queues more work. Leave it at the default unless you are waiting for
  something genuinely fast.
- **`WaitForObject` only tells you the object exists.** It does not tell you the object is
  interactive, on screen, unobscured, or finished animating. If a click after a successful wait
  does nothing, the wait was not the problem — see *Waiting for the right thing* below.

```eval_rst
.. note::
    ``WaitForCurrentSceneToBe`` uses an ``interval`` default of ``1`` second rather than ``0.5``.
```

### The implicit timeout caveat

`SetImplicitTimeout` supplies a default timeout for waits that did not specify one. It is off by
default.

It is applied **only when the call's `timeout` argument is still the default value of 20**. That
has a surprising consequence: passing `timeout=20` explicitly is indistinguishable from not
passing it, so the implicit timeout overrides it anyway. Passing any other value — including
`timeout=21` — opts that call out entirely.

```eval_rst
.. important::
    Treat ``SetImplicitTimeout`` as "change the default for calls that don't care", not as a
    global ceiling. If a specific wait needs a specific duration, pass it explicitly and pick a
    value other than 20.
```

## Delay after command

`SetDelayAfterCommand` inserts a pause after **every** command the driver sends. It defaults to
`0` and is measured in seconds.

It is a blunt instrument. A delay of 0.1 across a suite that issues ten thousand commands adds
roughly seventeen minutes of wall clock. Reach for it only to diagnose a suspected race — if a
small global delay makes a flaky test pass, you have learned the test is racing the app, and the
fix is a targeted wait, not the delay.

```eval_rst
.. warning::
    ``SetDelayAfterCommand`` is a debugging aid. Leaving it set in a committed suite trades a
    little flakiness for a lot of runtime, and hides the race rather than fixing it.
```

## Waiting for the right thing

The most common flaky pattern is waiting for existence when you need readiness.

```eval_rst
.. list-table::
   :widths: 44 56
   :header-rows: 1

   * - If the failure looks like…
     - Wait for…
   * - Object not found, intermittently
     - ``WaitForObject`` on the object itself.
   * - Object found, but the click does nothing
     - A property that means *ready* — ``WaitForComponentProperty`` on the component's
       ``interactable``, ``enabled`` or equivalent flag.
   * - Object found, click lands somewhere wrong
     - The animation to finish. Wait on a property that settles, such as a position or an
       animator state, rather than on the object.
   * - Works locally, fails in CI
     - The scene. ``WaitForCurrentSceneToBe`` before interacting, rather than assuming load order.
   * - First test in a run fails, the rest pass
     - The app to finish its startup. Wait for a known post-boot object, not a fixed sleep.
```

`WaitForComponentProperty` is the workhorse here and is under-used. Anything you can read with
`GetComponentProperty`, you can wait for.

### Inactive objects

`WaitForObject` takes an `enabled` parameter which defaults to `True`, meaning it matches only
objects that are **active in the hierarchy**. An object whose parent is inactive will not be
found, even though it exists.

If a wait fails for an object you can see in the Inspector, check whether an ancestor is inactive
before changing the selector. Passing `enabled=False` will find it — but acting on an inactive
object is usually not what you want, so treat that as a diagnostic rather than a fix.

## Frame rate and responsiveness

Two settings in AltTester® Desktop change how fast the app services commands, and therefore how
long the same wait takes:

- **Max framerate** — how many commands per second the app processes. Lower values make every
  command slower, which can turn a passing `timeout` into a failing one.
- **Live Update Framerate** — screenshots per second while Live Update is on. Live Update competes
  with command execution.

```eval_rst
.. note::
    If a suite times out only while someone is inspecting the app in AltTester® Desktop, Live
    Update is the first thing to check. It is not needed for a test run.
```

## A checklist for a flaky test

1. Which exception? `WaitTimeOutException` points at the test; `CommandResponseTimeoutException`
   points at the app.
2. Open the Inspector at the moment of failure and look for the object. Present? Active? In the
   scene you expected? See
   `Debugging a failing test <https://alttester.com/docs/desktop/latest/pages/guides/debugging.html>`_.
3. If it is present and active, you are waiting for existence when you need readiness. Move to
   `WaitForComponentProperty`.
4. If it is absent, check the scene and the parent chain before blaming the selector.
5. Only then consider raising `timeout`. A test that needs 60 seconds where 20 used to do is
   usually telling you something changed in the app.

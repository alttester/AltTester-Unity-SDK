# Command: ScrollMouseAndWait

## Description:

Simulate scroll mouse action in your game. This command waist for the action to finish. If you don't want to wait until the action to finish use [ScrollMouse]({{ site.baseurl }}/pages/commands/input-actions/scroll-mouse)

## Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| speed      |     float    |   false   | Set how fast to scroll. Positive values will scroll up and negative values will scroll down.|
| duration      |     float    |   false   | The time measured in seconds to move the mouse from current position to the set location.|

###### Observation: Since Java doesn't have optional parameters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **[AltScrollMouseParameters](../../other/java-builders.html#altscrollmouseparameters)** which we use the parameters mentioned. The java example will also show how to build such an object.

## Examples
```eval_rst
.. tabs::

    .. code-tab:: c#
        //TODO
    .. code-tab:: java
        //TODO


    .. code-tab:: py
        //TODO
```

/*
    Copyright(C) 2026 Altom Consulting
*/

package com.alttester.Commands.FindObject;

import com.alttester.AltObject;
import com.alttester.Commands.ObjectCommand.AltGetComponentPropertyParams;
import com.alttester.IMessageHandler;
import com.alttester.Utils;
import com.alttester.altTesterExceptions.WaitTimeOutException;
import com.google.gson.GsonBuilder;
import com.google.gson.JsonArray;

/**
 * Wait until there are no longer any objects that respect the given criteria or
 * times run out and
 * will throw an error.
 */
public class AltWaitForComponentProperty<T> extends AltBaseFindObject {
  /**
   * @param waitParams          the properties parameter for waiting the object
   * @param altObject           the AltObject element
   * @param property            the wanted value of the property
   * @param getPropertyAsString if true compares the property's value and the
   *                            actual value as
   *                            strings
   */
  private AltObject altObject;

  private AltWaitForComponentPropertyParams<T> waitParams;
  private T property;
  private Boolean getPropertyAsString = false;

  /**
   * @param messageHandler
   * @param altWaitForComponentPropertyParams
   */
  public AltWaitForComponentProperty(
      IMessageHandler messageHandler,
      AltWaitForComponentPropertyParams<T> altWaitForComponentPropertyParams,
      T property,
      AltObject altObject) {
    super(messageHandler);
    this.waitParams = altWaitForComponentPropertyParams;
    this.property = property;
    this.altObject = altObject;
  }

  public AltWaitForComponentProperty(
      IMessageHandler messageHandler,
      AltWaitForComponentPropertyParams<T> altWaitForComponentPropertyParams,
      T property,
      Boolean getPropertyAsString,
      AltObject altObject) {
    super(messageHandler);
    this.waitParams = altWaitForComponentPropertyParams;
    this.property = property;
    this.getPropertyAsString = getPropertyAsString;
    this.altObject = altObject;
  }

  public T Execute(Class<T> returnType) {
    double time = 0;
    String jsonElementToString = "";
    AltGetComponentPropertyParams getComponentPropertyParams = waitParams.getAltGetComponentPropertyParams();
    while (time < waitParams.getTimeout()) {
      logger.debug(
          "Waiting for element where name contains "
              + getComponentPropertyParams.getPropertyName()
              + "....");
      T propertyFound = altObject.getComponentProperty(getComponentPropertyParams, returnType);
      if (!getPropertyAsString && propertyFound.equals(property))
        return propertyFound;
      if (!(propertyFound instanceof JsonArray)) {
        String str = new GsonBuilder().serializeNulls().create().toJsonTree(propertyFound).toString();
        jsonElementToString = str.contains("\"") ? str : "\"" + str + "\"";
      } else {
        jsonElementToString = propertyFound.toString();
      }
      if (getPropertyAsString && jsonElementToString.equals(property.toString()))
        return propertyFound;
      if (propertyFound.toString().equals("0.0") && property.toString().equals("\"0\""))
        return propertyFound;

      Utils.sleepFor(waitParams.getInterval());
      time += waitParams.getInterval();
    }
    throw new WaitTimeOutException(
        "Property "
            + getComponentPropertyParams.getPropertyName()
            + " was "
            + jsonElementToString
            + " and was not "
            + property
            + " after "
            + waitParams.getTimeout()
            + " seconds");
  }
}

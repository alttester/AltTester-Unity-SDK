/*
    Copyright(C) 2026 Altom Consulting
*/

package com.alttester.Commands.ObjectCommand;

import com.alttester.AltObject;
import com.alttester.Commands.FindObject.AltBaseFindObject;
import com.alttester.Commands.FindObject.AltFindObjectsParams;
import com.alttester.IMessageHandler;

/**
 * Builder for finding the first object in the scene that respects the given
 * criteria. It's no
 * longer possible to search for object by name giving a path in the hierarchy.
 * For searching by
 * name, use searching by path.
 */
public class AltFindObjectFromObject extends AltBaseFindObject {
  private AltFindObjectsParams altFindObjectsParameters;
  private AltObject obj;

  /**
   * @param altFindObjectsParameters the properties parameter for finding the
   *                                 objects in a scene.
   * @param messageHandler
   */
  public AltFindObjectFromObject(
      IMessageHandler messageHandler,
      AltFindObjectsParams altFindObjectsParameters,
      AltObject obj) {
    super(messageHandler);
    this.altFindObjectsParameters = altFindObjectsParameters;
    this.obj = obj;
    this.altFindObjectsParameters.setCommandName("findObject");
  }

  public AltObject Execute() {
    altFindObjectsParameters.setPath(
        SetPathFromObject(
            obj, altFindObjectsParameters.getBy(), altFindObjectsParameters.getValue()));
    altFindObjectsParameters.setCameraPath(
        SetPath(altFindObjectsParameters.getCameraBy(), altFindObjectsParameters.getCameraValue()));
    SendCommand(altFindObjectsParameters);
    return ReceiveAltObject(altFindObjectsParameters);
  }
}

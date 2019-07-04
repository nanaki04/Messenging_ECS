using System;
using System.Linq;
using System.Text.RegularExpressions;
using Monads.Result;
using Unity.Entities;

namespace Messenging {

  public class LoggerMiddleWare : IMessageMiddleWare {
    public Func<Message, IResult<Message, Reason>> Apply(
      Func<Message, IResult<Message, Reason>> next,
      EntityCommandBuffer commandBuffer
    ) {
      return (message) => {
        if (!GlobalSettings.LoggingEnabled) {
          return next(message);
        }

        var type = message.GetType().Name;

        if (!GlobalSettings.LogFilter.Any(filter => Regex.IsMatch(type, filter))) {
          return next(message);
        }

        UnityEngine.Debug.Log("<color=lightblue>" + message.Serialize() + "</color>");
        return next(message);
      };
    }
  }

}

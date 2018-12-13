﻿using System;
using Akka.Actor;
using Akka.Event;
using shome.scene.core.model;

namespace shome.scene.akka.actors
{
    public class SceneActor:ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();

        public SceneActor(SceneConfig sceneConfig, KnownPaths knownPaths)
        {
            var salt = DateTime.Now.Ticks.ToString();
            foreach (var sceneAction in sceneConfig.Actions)
            {
                Context.ActorOf(ActionActor.Props(sceneAction, knownPaths), $"{sceneAction.Name}-{salt}");
            }
        }


        public static Props Props(SceneConfig sceneConfig, KnownPaths knownPaths)
        {
            return Akka.Actor.Props.Create(() => new SceneActor(sceneConfig, knownPaths));
        }

        protected override void PreStart()
        {
            base.PreStart();
            _logger.Debug($"SceneActor - '{this.Self.Path.Name}' starting");
        }

        protected override void PostStop()
        {
            _logger.Debug($"SceneActor - '{this.Self.Path.Name}' shutdown");
            base.PostStop();
        }

    }

}

# Media Spaces - Unity Sample

- Unity Version: 2023.2.19f1

This is a sample integrating Pharus tracking in a Unity application for Deep Space.

## Getting Started

- Open the Unity scene `SceneWithTracking` and press Play
- Start the Tracklink Simulator application ("tlsim", supplied in different repository)
  - tlsim's `absoluteW` and `absoluteH` settings should be the same as the `Stage X` and `Stage Y` properties in the `Tracker` object's `UnityPharusManager` component of the Unity scene
- In tlsim, right-click to create/destroy simulated blobs, and left-click to move them
- You should see the tracked rings move in the Unity game view

## How it works

`Tracker` is the object managing Pharus connections and has a component named `UnityPharusManager`. 

In the test scene, for each blob that is communicated to the Pharus manager (via multicast and the TrackLink protocol), the prefab defined in the Main object's `MainBhv` as `Subject Prefab` is instanced, and as the blobs are updated, so are these instances (via callbacks in `MainBhv`). The `MainBhv` of this test scene expects the subject prefab to have a `SubjectBhv` component; in its `Update()` function, this behaviour sets the position of the game object to the latest position information in the associated `Subject` structure.

For your application, you can change or replace the subject prefab. You can also change all the logic defined in the event callbacks of `MainBhv`; you may even decide not to use the `SubjectBhv` logic and update the positions of your visualization in another way.

It is highly recommended, however, to make use of the `TrackAdded`, `TrackUpdated` and `TrackRemoved` callbacks, like `MainBhv` does. This allows you to define your own logic without changing the Pharus classes.
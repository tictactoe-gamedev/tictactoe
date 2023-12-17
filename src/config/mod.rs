use bevy::{
    ecs::{event::Event, reflect::ReflectResource, system::Resource},
    reflect::Reflect,
};
#[cfg(feature = "with-inspector")]
use bevy_inspector_egui::{
    inspector_options::InspectorOptions, inspector_options::ReflectInspectorOptions,
};

#[derive(Debug, Clone, Copy, Resource, Reflect)]
#[reflect(Resource)]
#[cfg_attr(
    feature = "with-inspector",
    derive(InspectorOptions),
    reflect(InspectorOptions)
)]
pub struct WorldConfiguration {
    #[cfg_attr(
        feature = "with-inspector",
        inspector(min = -100, max = 100)
    )]
    pub x_min: i32,
    #[cfg_attr(
        feature = "with-inspector",
        inspector(min = -100, max = 100)
    )]
    pub x_max: i32,
    #[cfg_attr(
        feature = "with-inspector",
        inspector(min = -100, max = 100)
    )]
    pub z_min: i32,
    #[cfg_attr(
        feature = "with-inspector",
        inspector(min = -100, max = 100)
    )]
    pub z_max: i32,
}

impl Default for WorldConfiguration {
    fn default() -> Self {
        Self {
            x_min: -10,
            x_max: 10,
            z_min: -10,
            z_max: 10,
        }
    }
}

#[derive(Debug, Event)]
pub struct WorldConfigurationChanged;

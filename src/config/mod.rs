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

impl WorldConfiguration {
    pub fn world_length(&self) -> u32 {
        self.x_min.abs_diff(self.x_max + 1)
    }

    pub fn world_width(&self) -> u32 {
        self.z_min.abs_diff(self.z_max + 1)
    }
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

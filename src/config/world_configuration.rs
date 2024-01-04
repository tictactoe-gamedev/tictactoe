use bevy::{
    ecs::{event::Event, reflect::ReflectResource, system::Resource},
    reflect::Reflect,
};
#[cfg(feature = "with-inspector")]
use bevy_inspector_egui::{
    inspector_options::InspectorOptions, inspector_options::ReflectInspectorOptions,
};
use serde::Deserialize;

#[derive(Debug, Clone, Copy, Resource, Reflect, Deserialize)]
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
    pub y_min: i32,
    #[cfg_attr(
        feature = "with-inspector",
        inspector(min = -100, max = 100)
    )]
    pub y_max: i32,
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
    #[cfg_attr(feature = "with-inspector", inspector(min = 0, max = 100))]
    pub max_fox_count: u32,
}

impl WorldConfiguration {
    pub fn world_length(&self) -> u32 {
        self.x_min.abs_diff(self.x_max + 1)
    }

    pub fn world_height(&self) -> u32 {
        self.y_min.abs_diff(self.y_max + 1)
    }

    pub fn world_width(&self) -> u32 {
        self.z_min.abs_diff(self.z_max + 1)
    }
}

impl Default for WorldConfiguration {
    fn default() -> Self {
        Self {
            x_min: -4,
            x_max: 4,
            y_min: 0,
            y_max: 9,
            z_min: -4,
            z_max: 4,
            max_fox_count: 1,
        }
    }
}

#[derive(Debug, Event)]
pub struct WorldConfigurationChanged;

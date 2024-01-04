use bevy::{ecs::component::Component, math::Vec3};

#[derive(Debug, Default, Component)]
pub struct Fox {
    pub(super) target_position: Vec3,
    pub(super) time_from_last_action_taken: f64,
}

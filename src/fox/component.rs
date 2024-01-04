use bevy::{ecs::component::Component, math::Vec3};

#[derive(Debug, Default, Component)]
pub struct Fox {
    target_position: Vec3,
}

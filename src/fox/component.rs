use bevy::{animation::AnimationClip, asset::Handle, ecs::component::Component, math::Vec3};

#[derive(Debug, Default, Component)]
pub struct Fox {
    pub(super) target_position: Vec3,
    pub(super) time_from_last_action_taken: f32,
    pub(super) standing_animation_clip_handle: Handle<AnimationClip>,
    pub(super) walking_animation_clip_handle: Handle<AnimationClip>,
    pub(super) running_animation_clip_handle: Handle<AnimationClip>,
}

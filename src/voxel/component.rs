use bevy::ecs::component::Component;

#[derive(Debug, Component)]
pub struct Voxel {
    voxel_type: u64,
    amount: u64,
}

impl Voxel {
    pub fn new(voxel_type: u64, amount: u64) -> Self {
        Self { voxel_type, amount }
    }

    pub fn voxel_type(&self) -> &u64 {
        &self.voxel_type
    }

    pub fn amount(&self) -> &u64 {
        &self.amount
    }
}

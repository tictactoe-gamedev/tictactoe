use bevy::ecs::component::Component;

#[derive(Debug, Component)]
pub struct Voxel {
    position: (i32, i32, i32),
    voxel_type: u32,
    amount: u32,
}

impl Voxel {
    pub fn new(position: (i32, i32, i32), voxel_type: u32, amount: u32) -> Self {
        Self {
            position,
            voxel_type,
            amount,
        }
    }

    pub fn voxel_type(&self) -> &u32 {
        &self.voxel_type
    }

    pub fn amount(&self) -> &u32 {
        &self.amount
    }
}

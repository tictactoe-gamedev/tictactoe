// Fill out your copyright notice in the Description page of Project Settings.


#include "VoxelGameMode.h"
#include "VoxelActor.h"

AVoxelGameMode::AVoxelGameMode()
{
    DefaultPawnClass=nullptr;
}

void AVoxelGameMode::BeginPlay()
{
    Super::BeginPlay();
}
// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/GameModeBase.h"
#include "VoxelGameMode.generated.h"

/**
 * 
 */
UCLASS()
class VOXELPROJECT_API AVoxelGameMode : public AGameModeBase
{
	GENERATED_BODY()

public:
	AVoxelGameMode();

	virtual void BeginPlay() override;
	
};

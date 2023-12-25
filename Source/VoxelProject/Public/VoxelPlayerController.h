// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/PlayerController.h"
#include "VoxelPlayerController.generated.h"

/**
 * 
 */
UCLASS()
class VOXELPROJECT_API AVoxelPlayerController : public APlayerController
{
	GENERATED_BODY()
	
public:
    // Sets default values for this controller's properties
    AVoxelPlayerController();

	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = "Voxel")
	int32 VoxelType;

	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = "Voxel")
	int32 VoxelAmount;
	
	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = "Universe Size")
	int32 RandomX;

	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = "Universe Size")
	int32 RandomY;

	UPROPERTY(EditAnywhere, BlueprintReadOnly, Category = "Universe Size")
	int32 RandomZ;

protected:
    // Called when the game starts or when spawned
    virtual void BeginPlay() override;

    // Called to bind functionality to input
    virtual void SetupInputComponent() override;

private:
    void SpawnVoxel();

    TMap<int32, int32> VoxelTypeCounts;

    void UpdateVoxelTypeCount(int32 VoxelType);

	FString GetVoxelTypeName(int32 VoxelType);

	int32 MaxVoxelLimit;
    int32 TotalSpawnedVoxels;
};

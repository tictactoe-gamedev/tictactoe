// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "VoxelActor.generated.h"

UCLASS()
class AVoxelActor : public AActor
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	AVoxelActor();

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

public:
	UFUNCTION(BlueprintCallable, Category = "Voxel")
    void SpawnVoxel();

private:
	UFUNCTION(BlueprintCallable, Category = "Voxel")
	void SetRandomValues();

public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;

};

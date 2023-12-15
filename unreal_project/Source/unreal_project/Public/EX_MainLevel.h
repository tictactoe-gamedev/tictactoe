// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Engine/LevelScriptActor.h"
#include "EX_MainLevel.generated.h"

/**
 * 
 */
UCLASS()
class UNREAL_PROJECT_API AEX_MainLevel : public ALevelScriptActor
{
	GENERATED_BODY()
	
public:
	UPROPERTY(EditAnywhere)
	TObjectPtr<AActor> Sphere;

	UPROPERTY(EditAnywhere)
	FText MyText;
	
	virtual void BeginPlay() override;
};

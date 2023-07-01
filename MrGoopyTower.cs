using MelonLoader;
using BTD_Mod_Helper;
using System.Collections.Generic;
using System.Linq;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.TowerSets;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using BTD_Mod_Helper.Api.Display;
using UnityEngine;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Simulation.SMath;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
using Il2CppAssets.Scripts.Models.Map;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using MrGoopyTower;
using BTD_Mod_Helper.Api;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers.Weapons;
using Random = System.Random;
using HarmonyLib;
using Il2Cpp;

[assembly: MelonInfo(typeof(MrGoopyTower.MrGoopyTower), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace MrGoopyTower;

public class MrGoopyTower : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {
        ModHelper.Msg<MrGoopyTower>("mr goopy is redy to battle... (MrGoopyTower mod Loaded!)");
    }
    public override void OnTowerCreated(Tower tower, Entity target, Model modelToUse)
    {
        if (tower.model.name.Contains("MrGoopy"))
        {
            ModContent.GetAudioClip<MrGoopyTower>("morbinTime").Play();
        }
    }
    public class MrGoopy : ModTower
    {


        public override TowerSet TowerSet => TowerSet.Primary;
        public override string BaseTower => TowerType.DartMonkey;
        public override int Cost => 400;

        public override int TopPathUpgrades => 5;
        public override int MiddlePathUpgrades => 5;
        public override int BottomPathUpgrades => 5;

        public override ParagonMode ParagonMode => ParagonMode.Base555;
        public override string Description => "Mr goopy had enough of these bloons. It's goopin time.";
        public override string DisplayName => "Mr Goopy";

        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            towerModel.range += 15;
            var attackModel = towerModel.GetAttackModel();
            attackModel.range += 15;
            towerModel.ApplyDisplay<tier1Display>();

            var projectile = attackModel.weapons[0].projectile;
            projectile.display = Game.instance.model.GetTowerFromId("DartMonkey").display;
            var snipeSound2 = Game.instance.model.GetTowerFromId("Sauda").GetBehavior<CreateSoundOnSelectedModel>().Duplicate();
            snipeSound2.sound1.assetId = GetAudioSourceReference<MrGoopyTower>("boom");
            snipeSound2.sound2.assetId = GetAudioSourceReference<MrGoopyTower>("bruh");
            snipeSound2.sound3.assetId = GetAudioSourceReference<MrGoopyTower>("boom");
            snipeSound2.sound4.assetId = GetAudioSourceReference<MrGoopyTower>("rizz");
            snipeSound2.sound5.assetId = GetAudioSourceReference<MrGoopyTower>("rizz");
            snipeSound2.sound6.assetId = GetAudioSourceReference<MrGoopyTower>("bruh");
            snipeSound2.altSound1.assetId = GetAudioSourceReference<MrGoopyTower>("rizz");
            snipeSound2.altSound2.assetId = GetAudioSourceReference<MrGoopyTower>("boom");
            var placesound = Game.instance.model.GetTowerFromId("Sauda").GetBehavior<CreateSoundOnBloonEnterTrackModel>().Duplicate();
            placesound.moabSound.assetId = GetAudioSourceReference<MrGoopyTower>("siren");
            placesound.bfbSound.assetId = GetAudioSourceReference<MrGoopyTower>("siren");
            placesound.ddtSound.assetId = GetAudioSourceReference<MrGoopyTower>("siren");
            placesound.zomgSound.assetId = GetAudioSourceReference<MrGoopyTower>("siren");
            placesound.badSound.assetId = GetAudioSourceReference<MrGoopyTower>("siren");
            var exampleSoundModel = placesound.moabSound;
            towerModel.AddBehavior(snipeSound2);
            towerModel.AddBehavior(placesound);

        }
        public class tier1Display : ModDisplay
        {
            public override string BaseDisplay => GetDisplay(TowerType.BoomerangMonkey, 4, 0, 0);
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                node.RemoveBone("SuperMonkeyRig:Dart");
            }
        }
        public override bool IsValidCrosspath(int[] tiers) =>
            ModHelper.HasMod("UltimateCrosspathing") || base.IsValidCrosspath(tiers);

        public class StrongerMonkey : ModUpgrade<MrGoopy>
        {
            public override int Path => TOP;
            public override int Tier => 1;
            public override int Cost => 450;
            public override string Icon => "strongMonkey";
            public override string DisplayName => "Stronger Monkeys";

            public override string Description => "Stronger monkeys are thrown which pierce through more bloons.";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                var projectile = attackModel.weapons[0].projectile;
                projectile.display = Game.instance.model.GetTowerFromId("PatFusty 5").display;
                projectile.pierce += 5;
            }
        }
        public class Dynamonkeys : ModUpgrade<MrGoopy>
        {
            public override int Path => TOP;
            public override int Tier => 2;
            public override int Cost => 1000;
            public override string Icon => "dynamite";
            public override string DisplayName => "Dynamonkeys";

            public override string Description => "Dynamonkeys explode.";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                var projectile = attackModel.weapons[0].projectile;
                projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                projectile.display = Game.instance.model.GetTowerFromId("SniperMonkey-320").display;
                projectile.AddBehavior(Game.instance.model.GetTowerFromId("BombShooter-300").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate());
                projectile.AddBehavior(Game.instance.model.GetTowerFromId("BombShooter-300").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate());
                projectile.AddBehavior(Game.instance.model.GetTowerFromId("BombShooter-300").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileCollisionModel>().Duplicate());
                var proj = Game.instance.model.GetTowerFromId("BombShooter-003").GetAttackModel().weapons[0].projectile;
                projectile.AddBehavior(new CreateProjectileOnContactModel("Projectile_Create", proj, new ArcEmissionModel("ArcEmissionModel_", 3, 0.0f, 360.0f, null, false, false), true, false, false));
            }
        }
        public class DangerMonkeys : ModUpgrade<MrGoopy>
        {
            public override int Path => TOP;
            public override int Tier => 3;
            public override int Cost => 8000;
            public override string Icon => "danger";
            public override string DisplayName => "Dangerous Monkeys";

            public override string Description => "Monkeys are very dangerous to MOABS and camo bloons.";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                var projectile = attackModel.weapons[0].projectile;
                projectile.display = Game.instance.model.GetTowerFromId("SniperMonkey-050").display;
                projectile.AddBehavior(new DamageModifierForTagModel("moabmore", "Moabs", 2f, 12f, false, true));
                projectile.AddBehavior(new DamageModifierForTagModel("moabmore", "Camo", 1f, 3f, false, true));
                projectile.hasDamageModifiers = true;
                
            }
        }
        public class SmartMonkeys : ModUpgrade<MrGoopy>
        {
            public override int Path => TOP;
            public override int Tier => 4;
            public override int Cost => 15000;
            public override string Icon => "smartMonkey";
            public override string DisplayName => "Smart Monkeys";

            public override string Description => "Smart Monkeys do more damage and seek out bloons.";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                var projectile = attackModel.weapons[0].projectile;
                projectile.display = Game.instance.model.GetTowerFromId("Sauda 20").display;
                projectile.GetDamageModel().damage += 45;
                projectile.AddBehavior(Game.instance.model.GetTowerFromId("Adora 20").GetAttackModel().weapons[0].projectile.GetBehavior<AdoraTrackTargetModel>().Duplicate());
                projectile.GetBehavior<AdoraTrackTargetModel>().maximumSpeed *= 0.3f;
                projectile.GetBehavior<AdoraTrackTargetModel>().minimumSpeed *= 0.3f;
            }
        }
        public class GodMonkeyThrower : ModUpgrade<MrGoopy>
        {
            public override int Path => TOP;
            public override int Tier => 5;
            public override int Cost => 75000;
            public override string Icon => "goopyGod";
            public override string DisplayName => "Goopy God";

            public override string Description => "Mr Goopy throws out gods.";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
                var projectile = attackModel.weapons[0].projectile;
                projectile.display = Game.instance.model.GetTowerFromId("SuperMonkey-300").display;
                projectile.pierce += 20;
                projectile.GetDamageModel().damage *= 12;
                projectile.GetBehavior<AdoraTrackTargetModel>().maximumSpeed *= 0.3f;
                projectile.GetBehavior<AdoraTrackTargetModel>().minimumSpeed *= 0.3f;
                projectile.GetBehavior<AdoraTrackTargetModel>().lifespan *= 10;
            }
        }
        public class GoopySlap : ModUpgrade<MrGoopy>
        {
            public override int Path => MIDDLE;
            public override int Tier => 1;
            public override int Cost => 400;
            public override string Icon => "slapIcon";
            public override string DisplayName => "Goopy Slap";

            public override string Description => "Bloons close to Mr Goopy are slapped.";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var newattackModel = Game.instance.model.GetTowerFromId("Sauda").GetAttackModel().Duplicate();
                newattackModel.weapons[0].rate *= 3;
                newattackModel.weapons[0].projectile.pierce = 1;
                newattackModel.weapons[0].projectile.GetDamageModel().damage = 0;
                newattackModel.weapons[0].projectile.AddBehavior(new WindModel("_WindOnYoAss", 30, 40, 9000f, false, null, 0, null, 1));
                towerModel.AddBehavior(newattackModel);
            }
        }
        public class Minigunner : ModUpgrade<MrGoopy>
        {
            public override int Path => MIDDLE;
            public override int Tier => 2;
            public override int Cost => 4000;
            public override string Icon => "minigunner";
            public override string DisplayName => "Minigunner";

            public override string Description => "Loads up a minigun and starts to fire bullets fast.";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                var newWeapon = Game.instance.model.GetTowerFromId("SniperMonkey").GetAttackModel().weapons[0].Duplicate();
                newWeapon.rate = 0.05f;
                attackModel.AddWeapon(newWeapon);
                
            }
        }
        public class LemonadeStand : ModUpgrade<MrGoopy>
        {
            public override int Path => MIDDLE;
            public override int Tier => 3;
            public override int Cost => 7500;
            public override string Icon => "lemonadeStand";
            public override string DisplayName => "Lemonade Stand";

            public override string Description => "Scams people for lemonade very often";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                var bananaFarmAttackModel = Game.instance.model.GetTowerFromId("BananaFarm-003").GetAttackModel().Duplicate();
                bananaFarmAttackModel.name = "Tewbre_";
                bananaFarmAttackModel.weapons[0].projectile.GetBehavior<CashModel>().maximum = 10;
                bananaFarmAttackModel.weapons[0].projectile.GetBehavior<CashModel>().minimum = 10;
                bananaFarmAttackModel.weapons[0].GetBehavior<EmissionsPerRoundFilterModel>().count = 50;
                towerModel.AddBehavior(bananaFarmAttackModel);
            }
        }
        public class GoopyTime : ModUpgrade<MrGoopy>
        {
            public override int Path => MIDDLE;
            public override int Tier => 4;
            public override int Cost => 45000;
            public override string Icon => "grr";
            public override string DisplayName => "Goopin' Time";

            public override string Description => "It's goopin time.";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                var abilityModel = Game.instance.model.GetTowerFromId("BoomerangMonkey-040").GetAbility().Duplicate();
                abilityModel.GetBehavior<TurboModel>().extraDamage += 25;
                abilityModel.GetBehavior<TurboModel>().multiplier *= 0.5f;
                abilityModel.GetBehavior<TurboModel>().projectileDisplay.assetPath = CreatePrefabReference<waterDisplay>();
                abilityModel.GetBehavior<CreateSoundOnAbilityModel>().sound.assetId = GetAudioSourceReference<MrGoopyTower>("rar");
                abilityModel.cooldown *= 0.6f;
                abilityModel.icon = towerModel.icon;
                towerModel.AddBehavior(abilityModel);
            }
        }
        public class waterDisplay : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "mrGoopy-Icon");
            }
        }
        public class GoopyOnTop : ModUpgrade<MrGoopy>
        {
            public override int Path => MIDDLE;
            public override int Tier => 5;
            public override int Cost => 100000;
            public override string Icon => "cantRun";
            public override string DisplayName => "ez goopy";

            public override string Description => "Mr Goopy's ability is an ez win!";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                towerModel.GetAbility().GetBehavior<TurboModel>().extraDamage += 75;
                towerModel.GetAbility().cooldown *= 0.7f;
            }
        }
        public class FartSmella : ModUpgrade<MrGoopy>
        {
            public override int Path => BOTTOM;
            public override int Tier => 1;
            public override int Cost => 1000;
            public override string Icon => "fart";
            public override string DisplayName => "Flautence";

            public override string Description => "Farts fly out every few seconds. Extra stinky ones crit!";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                var newWeapon = Game.instance.model.GetTowerFromId("DartMonkey-004").GetAttackModel().weapons[0].Duplicate();
                newWeapon.rate *= 3.5f;
                newWeapon.projectile.pierce += 2;
                newWeapon.emission = new ArcEmissionModel("ArcEmissionModel_", 16, 0.0f, 360.0f, null, false, false);
                newWeapon.projectile.ApplyDisplay<fartDisplay>();
                attackModel.AddWeapon(newWeapon);
            }
        }
        public class fartDisplay : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "fart");
            }
        }
        public class freddyFazbear : ModUpgrade<MrGoopy>
        {
            public override int Path => BOTTOM;
            public override int Tier => 2;
            public override int Cost => 2000;
            public override string Icon => "freddy";
            public override string DisplayName => "Freddy Fazbear";

            public override string Description => "Freddy Fazbear is strong, scary, and most importantly, from FNAF!";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                var newWeapon = Game.instance.model.GetTowerFromId("DartMonkey-420").GetAttackModel().weapons[0].Duplicate();
                newWeapon.projectile.ApplyDisplay<fredDisplay>();
                attackModel.AddWeapon(newWeapon);
            }
        }
        public class fredDisplay : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "freddy");
            }
        }
        public class jamalJackson : ModUpgrade<MrGoopy>
        {
            public override int Path => BOTTOM;
            public override int Tier => 3;
            public override int Cost => 6000;
            public override string Icon => "jamal";
            public override string DisplayName => "Jamal";

            public override string Description => "Jamal jackson is ready for bloon busting!";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                var newWeapon = Game.instance.model.GetTowerFromId("BoomerangMonkey-420").GetAttackModel().weapons[0].Duplicate();
                newWeapon.projectile.ApplyDisplay<jamolDisplay>();
                attackModel.AddWeapon(newWeapon);
            }
        }
        public class jamolDisplay : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "jamal");
            }
        }
        public class micheal : ModUpgrade<MrGoopy>
        {
            public override int Path => BOTTOM;
            public override int Tier => 4;
            public override int Cost => 20000;
            public override string Icon => "micheal";
            public override string DisplayName => "Micheal Storen";

            public override string Description => "It's just micheal. Nothing too overpowered.";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                var newWeapon = Game.instance.model.GetTowerFromId("BoomerangMonkey-052").GetAttackModel().weapons[0].Duplicate();
                newWeapon.rate *= 0.5f;
                newWeapon.projectile.ApplyDisplay<michealDisplay>();
                attackModel.AddWeapon(newWeapon);
            }
        }
        public class michealDisplay : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "micheal");
            }
        }
        public class CarlWheezer : ModUpgrade<MrGoopy>
        {
            public override int Path => BOTTOM;
            public override int Tier => 5;
            public override int Cost => 120000;
            public override string Icon => "carl";
            public override string DisplayName => "Carl Wheezer Collab";

            public override string Description => "Carl Wheezer comes to help Mr. Goopy, and lets just say it gets out of hand.";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                var newWeapon = Game.instance.model.GetTowerFromId("SuperMonkey-052").GetAttackModel().weapons[0].Duplicate();
                newWeapon.rate *= 0.5f;
                newWeapon.projectile.ApplyDisplay<carlDisplay>();
                attackModel.AddWeapon(newWeapon);
            }
        }
        public class carlDisplay : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "carl");
            }
        }
        public class MrGoopyDrawers : ModParagonUpgrade<MrGoopy>
        {
            public override int Cost => 550000;
            public override string Description => "Goopy is the best no doubt can't deny is better than anything but don't ask me why";
            public override string DisplayName => "Goopy is the best";
            public override string Portrait => "Icon";
            public override string Icon => "Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.range += 100;
                var attackModel = towerModel.GetAttackModel();
                attackModel.range += 100;
                foreach (var weaponModel in towerModel.GetWeapons())
                {
                    weaponModel.rate *= 0.25f;
                }
                var newWeapon = Game.instance.model.GetTowerFromId("DartlingGunner-050").GetAttackModel().weapons[0].Duplicate();
                newWeapon.rate *= 0.15f;
                newWeapon.projectile.ApplyDisplay<michealDisplay>();
                attackModel.AddWeapon(newWeapon);
            }
        }
    }

}
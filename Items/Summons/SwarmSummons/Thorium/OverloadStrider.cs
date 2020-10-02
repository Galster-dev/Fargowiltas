﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Fargowiltas.Items.Summons.SwarmSummons.Thorium
{
    [Autoload(false)]
    public class OverloadStrider : ModItem
    {
        private readonly Mod thorium = Fargowiltas.FargosGetMod("ThoriumMod");

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cryo Egg");
            Tooltip.SetDefault("Summons several Borean Striders");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 100;
            item.value = 1000;
            item.rare = ItemRarityID.Pink;
            item.useAnimation = 30;
            item.useTime = 30;
            item.useStyle = ItemUseStyleID.HoldUp;
            item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return !Fargowiltas.SwarmActive;
        }

        public override bool UseItem(Player player)
        {
            Fargowiltas.SwarmActive = true;
            Fargowiltas.SwarmTotal = 10 * player.inventory[player.selectedItem].stack;
            Fargowiltas.SwarmKills = 0;

            // Kill whole stack
            player.inventory[player.selectedItem].stack = 0;

            if (Fargowiltas.SwarmTotal <= 20)
            {
                Fargowiltas.SwarmSpawned = Fargowiltas.SwarmTotal;
            }
            else if (Fargowiltas.SwarmTotal <= 100)
            {
                Fargowiltas.SwarmSpawned = 20;
            }
            else
            {
                Fargowiltas.SwarmSpawned = 40;
            }

            for (int i = 0; i < Fargowiltas.SwarmSpawned; i++)
            {
                //int boss = NPC.NewNPC((int)player.position.X + Main.rand.Next(-1000, 1000), (int)player.position.Y + Main.rand.Next(-1000, -400), thorium.NPCType("BoreanStrider"));
                //Main.npc[boss].GetGlobalNPC<FargoGlobalNPC>().SwarmActive = true;
            }

            if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("A wave of frozen air begins howling!"), new Color(175, 75, 255));
            }
            else
            {
                Main.NewText("A wave of frozen air begins howling!", 175, 75, 255);
            }

            SoundEngine.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(thorium, "StriderTear");
            recipe.AddIngredient(null, "Overloader");
            recipe.AddTile(TileID.DemonAltar);

            recipe.Register();
        }
    }
}
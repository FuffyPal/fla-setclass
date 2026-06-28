using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerRoles;
using UnityEngine;

namespace Flasetclass.Commands
{
    /// <summary>
    /// Bir birimin tum bilgilerini tutan kucuk veri sinifi.
    /// </summary>
    public class UnitProfile
    {
        public string Tag { get; set; }
        public string Info { get; set; }

        /// <summary>
        /// Envantere konacak normal esyalar (silah, zirh, keycard, medkit vs - mühimmat DEGIL).
        /// </summary>
        public List<ItemType> Items { get; set; } = new List<ItemType>();

        /// <summary>
        /// Mühimmat ayrica verilir, normal esya slotu kullanmaz.
        /// Key = mühimmat turu, Value = miktar.
        /// </summary>
        public Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>();

        /// <summary>
        /// Oyuncunun atanacagi gercek oyun rolu (asker, D-Class, bilim insani vs).
        /// </summary>
        public RoleTypeId Role { get; set; }
    }

    /// <summary>
    /// Konsola ".setclass id birim [bilgi]" yazinca calisacak komut.
    /// Ornek: .setclass 5 tau-1
    /// Ornek (ozel bilgi ile): .setclass 5 tau-1 Sahada gorevli birim
    /// </summary>
    [CommandHandler(typeof(ClientCommandHandler))]
    public class SetClassCommand : ICommand
    {
        public string Command => "setclass";

        public string[] Aliases => Array.Empty<string>();

        public string Description => "Kullanim: setclass <id> <birim> [bilgi]. Oyuncuya rol, etiket, bilgi, esya ve muhimmat verir.";

        // Not: Oyun ici "Cavus" / "Albay" rutbe isimleri ile enum'daki KeycardMTFOperative / KeycardMTFCaptain
        // isimleri farkli ama ayni karta karsilik geliyor (Operative = oyun ici "Sergeant" karti).
        private static readonly Dictionary<string, UnitProfile> Units = new Dictionary<string, UnitProfile>(StringComparer.OrdinalIgnoreCase)
        {
            {
                "tau-5", new UnitProfile
                {
                    Tag = "MTF Tau-5",
                    Info = "Samsara",
                    Role = RoleTypeId.NtfCaptain,
                    Items = new List<ItemType>
                    {
                        ItemType.KeycardMTFCaptain, ItemType.GunRevolver, ItemType.MicroHID,
                        ItemType.GunE11SR, ItemType.ArmorHeavy, ItemType.SCP500,
                        ItemType.Adrenaline, ItemType.Adrenaline, ItemType.Medkit, ItemType.Painkillers,
                    },
                    Ammo = new Dictionary<AmmoType, ushort>
                    {
                        { AmmoType.Ammo44Cal, 30 },
                        { AmmoType.Nato556, 120 },
                    },
                }
            },
            {
                "tau-1", new UnitProfile
                {
                    Tag = "MTF Tau-1",
                    Info = "Büyük Birader",
                    Role = RoleTypeId.NtfSergeant,
                    Items = new List<ItemType>
                    {
                        ItemType.KeycardMTFOperative, ItemType.GunCOM15, ItemType.GunE11SR,
                        ItemType.ArmorCombat, ItemType.SCP1344, ItemType.Adrenaline, ItemType.Medkit, ItemType.Painkillers,
                    },
                    Ammo = new Dictionary<AmmoType, ushort>
                    {
                        { AmmoType.Nato9, 120 },
                        { AmmoType.Nato556, 120 },
                    },
                }
            },
            {
                "pi-43", new UnitProfile
                {
                    Tag = "MTF Pi-43",
                    Info = "Çöp-Girdi-Çöp-Çıktı",
                    Role = RoleTypeId.NtfSergeant,
                    Items = new List<ItemType>
                    {
                        ItemType.KeycardMTFOperative, ItemType.GunCOM15, ItemType.GunFSP9,
                        ItemType.ArmorCombat, ItemType.Medkit, ItemType.Painkillers,
                    },
                    Ammo = new Dictionary<AmmoType, ushort>
                    {
                        { AmmoType.Nato9, 120 },
                    },
                }
            },
            {
                "beta-1", new UnitProfile
                {
                    Tag = "MTF Beta-1",
                    Info = "Dağlayıcılar",
                    Role = RoleTypeId.NtfSergeant,
                    Items = new List<ItemType>
                    {
                        ItemType.KeycardMTFOperative, ItemType.GunCOM15, ItemType.GunShotgun,
                        ItemType.ArmorCombat, ItemType.Medkit, ItemType.Painkillers,
                    },
                    Ammo = new Dictionary<AmmoType, ushort>
                    {
                        { AmmoType.Ammo44Cal, 40 },
                    },
                }
            },
            {
                "delta-4", new UnitProfile
                {
                    Tag = "MTF Delta-4",
                    Info = "Dakik Adamlar",
                    Role = RoleTypeId.NtfSergeant,
                    Items = new List<ItemType>
                    {
                        ItemType.KeycardMTFOperative, ItemType.GunCOM15, ItemType.ParticleDisruptor,
                        ItemType.GunE11SR, ItemType.ArmorCombat, ItemType.Medkit, ItemType.Painkillers,
                    },
                    Ammo = new Dictionary<AmmoType, ushort>
                    {
                        { AmmoType.Nato9, 120 },
                        { AmmoType.Nato556, 120 },
                    },
                }
            },
            {
                "rho-9", new UnitProfile
                {
                    Tag = "MTF Rho-9",
                    Info = "Teknik Destek",
                    Role = RoleTypeId.NtfSergeant,
                    Items = new List<ItemType>
                    {
                        ItemType.KeycardMTFOperative, ItemType.GunCOM15, ItemType.GunCrossvec,
                        ItemType.ArmorCombat, ItemType.Medkit, ItemType.Painkillers,
                    },
                    Ammo = new Dictionary<AmmoType, ushort>
                    {
                        { AmmoType.Nato9, 120 },
                    },
                }
            },
            {
                "mu-13", new UnitProfile
                {
                    Tag = "MTF Mu-13",
                    Info = "Hayalet Avcıları",
                    Role = RoleTypeId.NtfCaptain,
                    Items = new List<ItemType>
                    {
                        ItemType.KeycardMTFCaptain, ItemType.GunRevolver, ItemType.ParticleDisruptor,
                        ItemType.GunE11SR, ItemType.ArmorHeavy, ItemType.SCP500,
                        ItemType.Adrenaline, ItemType.Medkit, ItemType.Painkillers,
                    },
                    Ammo = new Dictionary<AmmoType, ushort>
                    {
                        { AmmoType.Ammo44Cal, 30 },
                        { AmmoType.Nato556, 120 },
                    },
                }
            },
            {
                "zeta-9", new UnitProfile
                {
                    Tag = "MTF Zeta-9",
                    Info = "Köstebek Fareleri",
                    Role = RoleTypeId.NtfSergeant,
                    Items = new List<ItemType>
                    {
                        ItemType.KeycardMTFOperative, ItemType.GunCOM15, ItemType.GunCrossvec,
                        ItemType.ArmorCombat, ItemType.Medkit, ItemType.Painkillers,
                    },
                    Ammo = new Dictionary<AmmoType, ushort>
                    {
                        { AmmoType.Nato9, 120 },
                    },
                }
            },
            {
                "alpha-9", new UnitProfile
                {
                    Tag = "MTF Alpha-9",
                    Info = "Son Umut",
                    Role = RoleTypeId.NtfSergeant,
                    Items = new List<ItemType>
                    {
                        ItemType.KeycardMTFOperative, ItemType.GunCOM15, ItemType.GunFRMG0,
                        ItemType.ArmorHeavy, ItemType.SCP500, ItemType.Adrenaline,
                        ItemType.Medkit, ItemType.Painkillers,
                    },
                    Ammo = new Dictionary<AmmoType, ushort>
                    {
                        { AmmoType.Nato9, 120 },
                        { AmmoType.Nato556, 120 },
                    },
                }
            },
            {
                "beta-7", new UnitProfile
                {
                    Tag = "MTF Beta-7",
                    Info = "Deli Şapkacılar",
                    Role = RoleTypeId.NtfSergeant,
                    Items = new List<ItemType>
                    {
                        ItemType.KeycardMTFOperative, ItemType.GunCOM15, ItemType.ParticleDisruptor,
                        ItemType.GunE11SR, ItemType.ArmorCombat, ItemType.Adrenaline,
                        ItemType.Medkit, ItemType.Painkillers,
                    },
                    Ammo = new Dictionary<AmmoType, ushort>
                    {
                        { AmmoType.Nato9, 120 },
                        { AmmoType.Nato556, 120 },
                    },
                }
            },
            {
                "nu-7-as", new UnitProfile
                {
                    Tag = "MTF Nu-7",
                    Info = "Çekiç Aşağı",
                    Role = RoleTypeId.NtfSergeant,
                    Items = new List<ItemType>
                    {
                        ItemType.KeycardMTFOperative, ItemType.GunCOM15, ItemType.GunLogicer,
                        ItemType.ArmorHeavy, ItemType.Medkit, ItemType.Painkillers,
                    },
                    Ammo = new Dictionary<AmmoType, ushort>
                    {
                        { AmmoType.Nato762, 120 },
                    },
                }
            },
            {
                "nu-7-al", new UnitProfile
                {
                    Tag = "MTF Nu-7",
                    Info = "Çekiç Aşağı",
                    Role = RoleTypeId.NtfCaptain,
                    Items = new List<ItemType>
                    {
                        ItemType.KeycardMTFCaptain, ItemType.GunRevolver, ItemType.GunFRMG0,
                        ItemType.GunLogicer, ItemType.ArmorHeavy, ItemType.SCP500,
                        ItemType.Adrenaline, ItemType.Adrenaline, ItemType.Medkit, ItemType.Painkillers,
                    },
                    Ammo = new Dictionary<AmmoType, ushort>
                    {
                        { AmmoType.Ammo44Cal, 30 },
                        { AmmoType.Nato762, 120 },
                    },
                }
            },
            {
                "epsilon-11-as", new UnitProfile
                {
                    Tag = "MTF Epsilon-11",
                    Info = "Dokuz Kuyruklu Tilki",
                    Role = RoleTypeId.NtfSergeant,
                    Items = new List<ItemType>
                    {
                        ItemType.KeycardMTFOperative, ItemType.GunCOM15, ItemType.GunE11SR,
                        ItemType.ArmorCombat, ItemType.Medkit, ItemType.Painkillers,
                    },
                    Ammo = new Dictionary<AmmoType, ushort>
                    {
                        { AmmoType.Nato9, 120 },
                        { AmmoType.Nato556, 120 },
                    },
                }
            },
            {
                "epsilon-11-t", new UnitProfile
                {
                    Tag = "MTF Epsilon-11",
                    Info = "Dokuz Kuyruklu Tilki",
                    Role = RoleTypeId.NtfSergeant,
                    Items = new List<ItemType>
                    {
                        ItemType.KeycardMTFOperative, ItemType.GunCOM15, ItemType.GunE11SR,
                        ItemType.ArmorCombat, ItemType.Adrenaline, ItemType.Medkit, ItemType.Painkillers,
                    },
                    Ammo = new Dictionary<AmmoType, ushort>
                    {
                        { AmmoType.Nato9, 120 },
                        { AmmoType.Nato556, 120 },
                    },
                }
            },
            {
                "epsilon-11-al", new UnitProfile
                {
                    Tag = "MTF Epsilon-11",
                    Info = "Dokuz Kuyruklu Tilki",
                    Role = RoleTypeId.NtfCaptain,
                    Items = new List<ItemType>
                    {
                        ItemType.KeycardMTFCaptain, ItemType.GunRevolver, ItemType.GunFRMG0,
                        ItemType.GunE11SR, ItemType.ArmorHeavy, ItemType.SCP500,
                        ItemType.Adrenaline, ItemType.Medkit, ItemType.Painkillers,
                    },
                    Ammo = new Dictionary<AmmoType, ushort>
                    {
                        { AmmoType.Ammo44Cal, 30 },
                        { AmmoType.Nato556, 120 },
                    },
                }
            },
            {
                "tesis-yöneticisi", new UnitProfile
                {
                    Tag = "Tesis Yöneticisi",
                    Info = "Tesis Yöneticisi",
                    Role = RoleTypeId.Scientist,
                    Items = new List<ItemType>
                    {
                        ItemType.KeycardFacilityManager, ItemType.GunRevolver,
                        ItemType.ArmorLight, ItemType.SCP500, ItemType.Painkillers,
                    },
                    Ammo = new Dictionary<AmmoType, ushort>
                    {
                        { AmmoType.Ammo44Cal, 30 },
                    },
                }
            },
            {
                "baş-bilim-insanı", new UnitProfile
                {
                    Tag = "Baş Bilim İnsanı",
                    Info = "Araştırma Mühendisi",
                    Role = RoleTypeId.Scientist,
                    Items = new List<ItemType>
                    {
                        ItemType.KeycardResearchCoordinator, ItemType.GunCOM15,
                        ItemType.ArmorLight, ItemType.Medkit, ItemType.Painkillers,
                    },
                    Ammo = new Dictionary<AmmoType, ushort>
                    {
                        { AmmoType.Nato9, 120 },
                    },
                }
            },
            {
                "bilim-insanı", new UnitProfile
                {
                    Tag = "Bilim İnsanı",
                    Info = "Bilim Adamı",
                    Role = RoleTypeId.Scientist,
                    Items = new List<ItemType>
                    {
                        ItemType.KeycardScientist, ItemType.ArmorLight, ItemType.Medkit,
                    },
                    Ammo = new Dictionary<AmmoType, ushort>(),
                }
            },
            {
                "tesis-görevlisi", new UnitProfile
                {
                    Tag = "Tesis Görevlisi",
                    Info = "Nefer",
                    Role = RoleTypeId.FacilityGuard,
                    Items = new List<ItemType>
                    {
                        ItemType.KeycardGuard, ItemType.GunCOM15, ItemType.GunCrossvec,
                        ItemType.ArmorCombat, ItemType.Medkit, ItemType.Painkillers,
                    },
                    Ammo = new Dictionary<AmmoType, ushort>
                    {
                        { AmmoType.Nato9, 120 },
                    },
                }
            },
            {
                "kayıt-karşılama", new UnitProfile
                {
                    Tag = "Kayıt Karşılama",
                    Info = "MGG Çavuş",
                    Role = RoleTypeId.FacilityGuard,
                    Items = new List<ItemType>
                    {
                        ItemType.KeycardGuard, ItemType.GunCOM18, ItemType.GunE11SR,
                        ItemType.ArmorCombat, ItemType.Medkit, ItemType.Painkillers,
                    },
                    Ammo = new Dictionary<AmmoType, ushort>
                    {
                        { AmmoType.Nato9, 120 },
                        { AmmoType.Nato556, 120 },
                    },
                }
            },
        };

        /// <summary>
        /// Komut calistirildiginde buradaki kod calisir.
        /// </summary>
        /// <param name="arguments">Komuttan sonra yazilan ek kelimeler.</param>
        /// <param name="sender">Komutu kim yazdiysa o.</param>
        /// <param name="response">Geri donecek mesaj.</param>
        /// <returns>Komutun basarili calisip calismadigi.</returns>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count < 2)
            {
                response = "Kullanim: .setclass <id> <birim> [bilgi]  (ornek: .setclass 5 tau-1)";
                return false;
            }

            string idArgument = arguments.At(0);
            string unitArgument = arguments.At(1);

            if (!int.TryParse(idArgument, out int playerId))
            {
                response = $"'{idArgument}' gecerli bir oyuncu ID'si degil.";
                return false;
            }

            if (!Player.TryGet(playerId, out Player target))
            {
                response = $"ID {playerId} ile bir oyuncu bulunamadi.";
                return false;
            }

            if (!Units.TryGetValue(unitArgument, out UnitProfile profile))
            {
                response = $"'{unitArgument}' bilinen bir birim degil. Gecerli birimler: {string.Join(", ", Units.Keys)}";
                return false;
            }

            string infoArgument = arguments.Count > 2
                ? string.Join(" ", arguments.Skip(2))
                : null;

            // Rol degisiminden ONCE konumu kaydet, cunku Role.Set oyuncuyu spawn noktasina tasiyor.
            Vector3 originalPosition = target.Position;

            // 1) Oyuncunun GERCEK rolunu degistir (asker, D-Class, bilim insani vs).
            target.Role.Set(profile.Role);

            // 2) Oyuncuyu rol degismeden once bulundugu konuma geri tasi (spawn noktasina gitmesin).
            target.Position = originalPosition;

            // 3) Ismin basina etiket ekle.
            target.CustomName = $"{profile.Tag} {target.Nickname}";

            // 4) CustomInfo: admin elle yazdiysa onu kullan, yazmadiysa birimin varsayilanini kullan.
            string infoToApply = !string.IsNullOrEmpty(infoArgument) ? infoArgument : profile.Info;
            if (!string.IsNullOrEmpty(infoToApply))
                target.CustomInfo = infoToApply;

            // 5) Envanteri tek seferde sifirla ve sadece bizim verdigimiz esyalari koy.
            target.ResetInventory(profile.Items);

            // 6) Muhimmati ayrica ver (mühimmat envanter slotu kullanmiyor, ayri bir havuzda tutuluyor).
            target.ClearAmmo();
            foreach (KeyValuePair<AmmoType, ushort> ammoEntry in profile.Ammo)
                target.SetAmmo(ammoEntry.Key, ammoEntry.Value);

            string itemsList = string.Join(", ", profile.Items.Select(i => i.ToString()));
            response = $"{target.Nickname} (ID: {playerId}) artik '{target.CustomName}' ({profile.Role}) oldu. Bilgi: '{infoToApply}'. Esyalar: {itemsList}";
            return true;
        }
    }
}
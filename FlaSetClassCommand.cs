using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
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
    ///
    /// Gerekli permission: flasetclass.use
    /// Permissions.yml ornegi:
    ///   default:
    ///     - flasetclass.use   # herkese acmak istersen
    ///   moderator:
    ///     - flasetclass.use
    /// </summary>
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SetClassCommand : ICommand
    {
        public string Command => "setclass";
        public string[] Aliases => Array.Empty<string>();
        public string Description => "Kullanim: setclass <id> <birim> [bilgi]. Oyuncuya rol, etiket, bilgi, esya ve muhimmat verir. Gerekli yetki: flasetclass.use";

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
                "tesis-görevlisi", new UnitProfile  // düzeltildi: "tesis-görevlisis" -> "tesis-görevlisi"
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
            // --- YETKİ KONTROLÜ ---
            if (!sender.CheckPermission("flasetclass.use"))
            {
                response = "Bu komutu kullanmak için 'flasetclass.use' yetkisine sahip olman gerekiyor.";
                return false;
            }

            // En az 2 arguman gerekiyor: en az 1 ID + birim ismi.
            // Yeni format: .setclass <id1> [id2] [id3] ... <birim>
            // Son arguman HER ZAMAN birim ismidir, oncekiler ID'lerdir.
            if (arguments.Count < 2)
            {
                response = "Kullanim: .setclass <id1> [id2 id3 ...] <birim>  (ornek: .setclass 2 3 5 tau-1)";
                return false;
            }

            // Son arguman = birim ismi.
            string unitArgument = arguments.At(arguments.Count - 1);

            if (!Units.TryGetValue(unitArgument, out UnitProfile profile))
            {
                response = $"'{unitArgument}' bilinen bir birim degil. Gecerli birimler: {string.Join(", ", Units.Keys)}";
                return false;
            }

            // Birimd ismi haric kalan tum argumanlari ID olarak isle.
            List<string> idArguments = arguments.Take(arguments.Count - 1).ToList();

            List<string> successLines = new List<string>();
            List<string> failLines = new List<string>();

            foreach (string idStr in idArguments)
            {
                // Her ID'yi ayri ayri kontrol et; biri gecersizse digerlerini atlamadan devam et.
                if (!int.TryParse(idStr, out int playerId))
                {
                    failLines.Add($"'{idStr}' gecerli bir ID degil.");
                    continue;
                }

                if (!Player.TryGet(playerId, out Player target))
                {
                    failLines.Add($"ID {playerId} bulunamadi.");
                    continue;
                }

                ApplyProfile(target, profile);
                successLines.Add($"{target.Nickname} (ID: {playerId})");
            }

            // Sonuc ozeti olustur.
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (successLines.Count > 0)
                sb.AppendLine($"[OK] '{unitArgument}' atandi: {string.Join(", ", successLines)}");

            if (failLines.Count > 0)
                sb.AppendLine($"[HATA] {string.Join(" | ", failLines)}");

            response = sb.ToString().TrimEnd();
            return successLines.Count > 0;
        }

        /// <summary>
        /// Tek bir oyuncuya profili uygular: rol, konum koruma, isim, bilgi, envanter, muhimmat.
        /// Birden fazla ID destegi icin ayri bir metoda cikartildi, boylece her ID icin tekrarlanabilir.
        /// </summary>
        private static void ApplyProfile(Player target, UnitProfile profile)
        {
            // Rol degisiminden ONCE konumu kaydet.
            Vector3 originalPosition = target.Position;

            // 1) Gercek oyun rolunu degistir.
            target.Role.Set(profile.Role);

            // 2) Spawn noktasina gitmemesi icin eski konuma geri don.
            target.Position = originalPosition;

            // 3) Ismin basina etiket ekle.
            target.CustomName = $"{profile.Tag} {target.Nickname}";

            // 4) CustomInfo yaz.
            if (!string.IsNullOrEmpty(profile.Info))
                target.CustomInfo = profile.Info;

            // 5) Envanteri sifirla ve yeni esyalari ver.
            target.ResetInventory(profile.Items);

            // 6) Muhimmati ayrica ver.
            target.ClearAmmo();
            foreach (KeyValuePair<AmmoType, ushort> ammoEntry in profile.Ammo)
                target.SetAmmo(ammoEntry.Key, ammoEntry.Value);
        }
    }
}
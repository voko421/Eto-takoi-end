using Content.Shared.Corvax.TTS;
using Content.Shared.Humanoid;
using Content.Shared.StatusIcon;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Backmen.Changeling.Components;

[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]
public sealed partial class ChangelingComponent : Component
{
    public override bool SendOnlyToOwner => true;

    #region Prototypes

    [DataField] public List<SoundSpecifier?> SoundPool = new()
    {
        new SoundPathSpecifier("/Audio/Effects/gib1.ogg"),
        new SoundPathSpecifier("/Audio/Effects/gib2.ogg"),
        new SoundPathSpecifier("/Audio/Effects/gib3.ogg"),
    };

    [DataField] public SoundSpecifier ShriekSound = new SoundPathSpecifier("/Audio/Effects/changeling_shriek.ogg");

    [DataField] public float ShriekPower = 2.5f;

    public readonly List<EntProtoId> BaseChangelingActions = new()
    {
        "ActionEvolutionMenu",
        "ActionAbsorbDNA",
        "ActionStingExtractDNA",
        "ActionChangelingTransformCycle",
        "ActionChangelingTransform",
        "ActionEnterStasis",
        "ActionExitStasis",
    };

    /// <summary>
    ///     The status icon corresponding to the Changlings.
    /// </summary>

    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public ProtoId<StatusIconPrototype> StatusIcon { get; set; } = "HivemindFaction";

    #endregion

    public bool IsInStasis = false;

    public bool StrainedMusclesActive = false;

    public bool IsInLesserForm = false;

    [ViewVariables(VVAccess.ReadWrite)]
    public float VomitAmount { get; set; }= 15f;


    public Dictionary<string, EntityUid?> Equipment = new();

    /// <summary>
    ///     Amount of biomass changeling currently has.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float Biomass = 30f;

    /// <summary>
    ///     Maximum amount of biomass a changeling can have.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float MaxBiomass = 30f;

    /// <summary>
    ///     How much biomass should be removed per cycle.
    /// </summary>
    [DataField]
    public float BiomassDrain = 1f;

    /// <summary>
    ///     Current amount of chemicals changeling currently has.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float Chemicals = 100f;

    /// <summary>
    ///     Maximum amount of chemicals changeling can have.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float MaxChemicals = 100f;

    /// <summary>
    ///     Bonus chemicals regeneration. In case
    /// </summary>
    [DataField]
    public float BonusChemicalRegen = 0f;

    /// <summary>
    ///     Cooldown between chem regen events.
    /// </summary>
    public TimeSpan UpdateTimer = TimeSpan.Zero;
    public float UpdateCooldown = 1f;

    public float BiomassUpdateTimer = 0f;
    public float BiomassUpdateCooldown = 60f;

    [ViewVariables(VVAccess.ReadOnly)]
    public List<TransformData> AbsorbedDNA = new();
    /// <summary>
    ///     Index of <see cref="AbsorbedDNA"/>. Used for switching forms.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public int AbsorbedDNAIndex = 0;

    /// <summary>
    ///     Maximum amount of DNA a changeling can absorb.
    /// </summary>
    public int MaxAbsorbedDNA = 5;

    /// <summary>
    ///     Total absorbed DNA. Counts towards objectives.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public int TotalAbsorbedEntities = 0;

    /// <summary>
    ///     Total stolen DNA. Counts towards objectives.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public int TotalStolenDNA = 0;

    [ViewVariables(VVAccess.ReadOnly)]
    public TransformData? CurrentForm;

    [ViewVariables(VVAccess.ReadOnly)]
    public TransformData? SelectedForm;

    [ViewVariables(VVAccess.ReadWrite), AutoNetworkedField]
    public bool IsTransponder = false;
}

[DataDefinition]
public sealed partial class TransformData
{
    /// <summary>
    ///     Entity's name.
    /// </summary>
    [DataField]
    public string Name;

    /// <summary>
    ///     Entity's fingerprint, if it exists.
    /// </summary>
    [DataField]
    public string Fingerprint;

    /// <summary>
    ///     Entity's DNA.
    /// </summary>
    [DataField("dna")]
    public string DNA;

    [DataField("tts")]
    public ProtoId<TTSVoicePrototype> TTS;

    /// <summary>
    ///     Entity's humanoid appearance component.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly), NonSerialized]
    public Entity<HumanoidAppearanceComponent> Appearance;
}

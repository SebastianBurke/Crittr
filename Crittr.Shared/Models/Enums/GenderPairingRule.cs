namespace Crittr.Shared.Models.Enums;

public enum GenderPairingRule
{
    AnyGenderOk,           // default — gender doesn't affect cohabitation
    OneMalePerEnclosure,   // male+male conflict; lethality set by MaleMaleIsLethal
}

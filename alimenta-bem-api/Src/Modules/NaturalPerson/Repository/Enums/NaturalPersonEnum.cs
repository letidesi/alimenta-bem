using System.ComponentModel;

namespace AlimentaBem.Src.Modules.NaturalPerson.Repository.Enums;

public enum Gender
{
    [Description("Feminino")]
    Feminino,

    [Description("Masculino")]
    Masculino,

    [Description("Pessoa Não-binária,")]
    PessoaNaoBinaria,

    [Description("Prefiro Não Dizer,")]
    PrefiroNaoDizer
}

public enum SkinColor
{
    [Description("Branca")]
    Branca,

    [Description("Preta")]
    Preta,

    [Description("Amarela")]
    Amarela,

    [Description("Parda")]
    Parda,

    [Description("Asiática")]
    Asiatica,

    [Description("Indígena")]
    Indigena,

    [Description("Prefiro Não Dizer,")]
    PrefiroNaoDizer
}
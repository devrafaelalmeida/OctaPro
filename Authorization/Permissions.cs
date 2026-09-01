namespace OctaPro.Authorization;

public static class Permissions
{
    public const string AccessControlRead = "access_control.read";
    public const string AccessControlUpdate = "access_control.update";

    public const string CorporationRead = "corporation.read";
    public const string CorporationCreate = "corporation.create";
    public const string CorporationUpdate = "corporation.update";
    public const string CorporationDelete = "corporation.delete";

    public const string EntityRead = "entity.read";
    public const string EntityCreate = "entity.create";
    public const string EntityUpdate = "entity.update";
    public const string EntityDelete = "entity.delete";

    public const string InstallmentReverse = "installment.reverse";

    public const string JudicialProcessRead = "judicial_process.read";
    public const string JudicialProcessCreate = "judicial_process.create";
    public const string JudicialProcessUpdate = "judicial_process.update";
    public const string JudicialProcessArchive = "judicial_process.archive";
    public const string JudicialProcessDelete = "judicial_process.delete";

    public const string LegalFeeRead = "legal_fee.read";
    public const string LegalFeeCreate = "legal_fee.create";
    public const string LegalFeeUpdate = "legal_fee.update";
    public const string LegalFeeDelete = "legal_fee.delete";
    public const string LegalFeeAddInstallment = "legal_fee.add_installment";

    public const string SettlementRead = "settlement.read";
    public const string SettlementCreate = "settlement.create";
    public const string SettlementUpdate = "settlement.update";
    public const string SettlementDelete = "settlement.delete";
    public const string SettlementAddInstallment = "settlement.add_installment";

    public const string UserRead = "user.read";
    public const string UserCreate = "user.create";
    public const string UserUpdate = "user.update";
    public const string UserDelete = "user.delete";

    public static readonly string[] All =
    [
        AccessControlRead,
        AccessControlUpdate,
        CorporationRead,
        CorporationCreate,
        CorporationUpdate,
        CorporationDelete,
        EntityRead,
        EntityCreate,
        EntityUpdate,
        EntityDelete,
        InstallmentReverse,
        JudicialProcessRead,
        JudicialProcessCreate,
        JudicialProcessUpdate,
        JudicialProcessArchive,
        JudicialProcessDelete,
        LegalFeeRead,
        LegalFeeCreate,
        LegalFeeUpdate,
        LegalFeeDelete,
        LegalFeeAddInstallment,
        SettlementRead,
        SettlementCreate,
        SettlementUpdate,
        SettlementDelete,
        SettlementAddInstallment,
        UserRead,
        UserCreate,
        UserUpdate,
        UserDelete
    ];
}

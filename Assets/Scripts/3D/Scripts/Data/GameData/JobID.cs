using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class JobID : UniqueID {
    public static IEnumerable<NamedGuid> GetIDs() {
        return NamedGuid.GetIDs<JobID>();
    }

    public static IEnumerable<Guid> GetValues() {
        return NamedGuid.GetValues<JobID>();
    }

    public static JobID GetValue(string idName) {
        return new JobID(NamedGuid.GetValue<JobID>(idName));
    }

    public static bool ContainsID(string idName) {
        return NamedGuid.ContainsID<JobID>(idName);// GetIDs().Where(n => n.Name == idName).FirstOrDefault() != null;
    }

    public static readonly JobID None = new JobID("30a2a4e3b8614186962d3ca901c66d92");

    public static readonly JobID Tourist1 = new JobID("b54eb95f12aa4cdb845211fc37112201");
    public static readonly JobID Tourist2 = new JobID("b443622f7c2547d1a4f8eb564be355e8");
    public static readonly JobID Tourist3 = new JobID("887390d5a8214636ba99baf462544833");

    public static readonly JobID Cashier1 = new JobID("84e76c07a33240918a38368ed026f600");
	public static readonly JobID OpenCashier = new JobID("9031c100132c42c1957b3641b0fc8611");

    public static readonly JobID Guide1 = new JobID("9a4eddbaf3394ab8a239b46310c3a0c1");
    public static readonly JobID Guide2 = new JobID("c2a7fa5151064ae3b4c6c46d9e839194");

    public static readonly JobID TalentSeeker1 = new JobID("461162fe01fe480eac77218317dfc004");
    public static readonly JobID TalentSeeker2 = new JobID("f075aee8406b470a9de7591f7c4998f6");
    public static readonly JobID TalentSeeker3 = new JobID("73011d186a744a28b7a4dea64a1cd457");
    public static readonly JobID TalentSeeker4 = new JobID("8de68578bd5b4840ab769b8ef41773f1");

    public static readonly JobID Janitor1 = new JobID("a13185171da44989a288ee47a5e7928e");
    public static readonly JobID Janitor2 = new JobID("a002e470a14d4e62a5117e0e4fd61d7c");
    public static readonly JobID Janitor3 = new JobID("6939f054ce2849f29e9cb1aef65d79c3");
    public static readonly JobID OpenJanitor = new JobID("9123d2e2e10544c0925883d4bd7a8250");

    public static readonly JobID Student1 = new JobID("05a9ae2bb67743bca5ef97845caddd9c");
    public static readonly JobID PetFeeder1 = new JobID("98b3fa69e2dd49108c2008cdb08054ae");
    public static readonly JobID Volunteer1 = new JobID("aa51984ed1bd423f92a922104d2770c1");
    //will not be job in the long run
    public static readonly JobID Explorer = new JobID("18daba7f5b244a9fbe0e8f785b290102");
    public static readonly JobID ExploreCashier = new JobID("ed90f02bac9d4831aa527d6f647f2327");
    public static readonly JobID ExploreGuide = new JobID("c57ed5324e404afa9dc96f6ea11d0fcd");

    public static readonly JobID TestExplorer = new JobID("c45622b4c3ba4fad96373724cf7117c9");

    public static readonly JobID FreeExplore = new JobID("8dbc06e9e4f74c5791f9e403b49f0363");

    public JobID() : base() { }
    public JobID(string id) : base(id) { }
    public JobID(Guid id) : base(id) { }
}

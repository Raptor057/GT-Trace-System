namespace GT.Trace.Packaging.Infra.DataSources.Entities
{
    public class UARTICLE
    {
        public string ARKTSOC { get; set; } = "";

        public string ARKTCODART { get; set; } = "";

        public string ARKTCOMART { get; set; } = "";

        public string ARKTGESTIO { get; set; } = "";

        public string ARKTCONDI { get; set; } = "";

        public string ARKTEMPL { get; set; } = "";

        public string ARKCRITPAR { get; set; } = "";

        public string ARKMAGASIN { get; set; } = "";

        public string ARKNEXTOP { get; set; } = "";

        public string APKUPC { get; set; } = "";

        public string APKCONTENE { get; set; } = "";

        public int? APKSTDPACK { get; set; }

        public string APKSMKT { get; set; } = "";

        public string APKBOLSA { get; set; } = "";

        public string APKRATIO { get; set; } = "";

        public string ARKTPURCAT { get; set; } = "";

        public string ARKTPURFAM { get; set; } = "";

        public string ARKTPURMNG { get; set; } = "";

        public string ARKTPURBUY { get; set; } = "";

        public string APKHTS { get; set; } = "";

        public string APKFRACION { get; set; } = "";

        public string APKORIGEN { get; set; } = "";

        public string APKFTID { get; set; } = "";

        public int? APKAREAUSO { get; set; }

        public string APKCONT001 { get; set; } = "";

        public int? APKNPCECON { get; set; }

        public string APKBOLSA2 { get; set; } = "";

        public float? APKDIAPRUE { get; set; }

        public string APKOTD { get; set; } = "";

        public string APKSKIPLOT { get; set; } = "";

        public string ARKTRESP { get; set; } = "";

        public decimal? ARKTNBEMPR { get; set; }

        public int? ARKTPMIN { get; set; }

        public int? ARKTPMAX { get; set; }

        public string ARKPULL { get; set; } = "";

        public string ARKTPURSTA { get; set; } = "";

        public string ARKTPRULOT { get; set; } = "";

        public string ARKTTELECT { get; set; } = "";

        public string ARKTMASTER { get; set; } = "";

        public string ARKTNBID { get; set; } = "";

        public string ARKTGAREMU { get; set; } = "";

        public string ARKTCERTIF { get; set; } = "";

        public string ARKTINTAUT { get; set; } = "";

        //Agregado para obtener la pagina web 04/16/2024
        public string? ARKTSAVWWW { get; set; }

        public decimal? ARKTIMETO { get; set; }

        public int? APKNPCECO2 { get; set; }//Se agrego como nuevo para el uso de intercambio de cantidad de empaque entre cantidades en EZ RA: 5/31/2023

        public int PalletSize => APKNPCECON ?? 0;

        public int PalletSize2 => APKNPCECO2 ?? 0; //Se agrego como nuevo para el uso de intercambio de cantidad de empaque entre cantidades en EZ RA: 5/31/2023

        public int ContainerSize => APKSTDPACK ?? 0;

        public string MasterTypeCode => ARKTMASTER; //INFO: Aqui se declara MasterTypeCode como ARKTMASTER de cegid en la tabla UARTICLE.

        public string Suffix => APKFTID;
    }
}
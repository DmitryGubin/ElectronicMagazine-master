//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ElectronicMagazine
{
    using System;
    using System.Collections.Generic;
    
    public partial class Message
    {
        public int Id { get; set; }
        public int Id_Student { get; set; }
        public int Id_Teacher { get; set; }
        public string Report { get; set; }
        public string Author { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    
        public virtual Students Students { get; set; }
        public virtual Teachers Teachers { get; set; }
    }
}

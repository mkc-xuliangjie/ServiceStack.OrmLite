﻿using System;
using ServiceStack.OrmLite.Converters;

namespace ServiceStack.OrmLite.Firebird.Converters
{
    public class FirebirdGuidConverter : GuidConverter
    {
        public override string ColumnDefinition
        {
            get { return "VARCHAR(37)"; }
        }

        public override string ToQuotedString(Type fieldType, object value)
        {
            return string.Format("CAST('{0}' AS {1})", (Guid)value, ColumnDefinition);  // TODO : check this !!!
        }

        public override object FromDbValue(FieldDefinition fieldDef, object value)
        {
            return new Guid(value.ToString());
        }
    }

    public class FirebirdCompactGuidConverter : GuidConverter
    {
        public override string ColumnDefinition
        {
            get { return "CHAR(16) CHARACTER SET OCTETS"; }
        }

        public override string ToQuotedString(Type fieldType, object value)
        {
            return "X'" + ((Guid)value).ToString("N") + "'";
        }

        public override object FromDbValue(FieldDefinition fieldDef, object value)
        {
            //BitConverter.IsLittleEndian // TODO: check big endian

            byte[] raw = ((Guid)value).ToByteArray();
            return new Guid(System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt32(raw, 0)),
                System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt16(raw, 4)),
                System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt16(raw, 6)),
                raw[8], raw[9], raw[10], raw[11], raw[12], raw[13], raw[14], raw[15]);
        }
    }
}
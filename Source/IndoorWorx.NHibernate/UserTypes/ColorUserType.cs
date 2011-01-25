using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.UserTypes;
using System.Data;
using NHibernate;
using NHibernate.SqlTypes;
using System.Drawing;

namespace IndoorWorx.NHibernate.UserTypes
{
    public class ColorUserType : IUserType
    {
        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Disassemble(object value)
        {
            return value;
        }

        public new bool Equals(object x, object y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }
            return x.Equals(y);
        }

        public int GetHashCode(object x)
        {
            return x == null ? typeof(Color).GetHashCode() + 473 : x.GetHashCode();
        }

        public bool IsMutable
        {
            get
            {
                return true;
            }
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            var obj = NHibernateUtil.String.NullSafeGet(rs, names[0]);
            if (obj == null)
            {
                return null;
            }
            return ColorTranslator.FromHtml((string)obj);
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (value == null)
            {
                ((IDataParameter)cmd.Parameters[index]).Value = DBNull.Value;
            }
            else
            {
                ((IDataParameter)cmd.Parameters[index]).Value = ColorTranslator.ToHtml((Color)value);
            }
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public Type ReturnedType
        {
            get
            {
                return typeof(Color);
            }
        }

        public SqlType[] SqlTypes
        {
            get
            {
                return new[] { new SqlType(DbType.StringFixedLength) };
            }
        }
    }
}

using System;
using SteakBot.Core.Objects.Enums;

namespace SteakBot.Core.Objects
{
    public class MemeCommand : IEquatable<MemeCommand>
    {
        public MemeResultType ResultType { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public MemeCommand(MemeResultType resultType, string name, string value, string description)
        {
            ResultType = resultType;
            Name = name;
            Value = value;
            Description = description;
        }

        #region IEquitable

        public override bool Equals(object other)
        {
            var item = other as MemeCommand;
            return item != null && Equals(item);
        }

        public bool Equals(MemeCommand other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if(ReferenceEquals(other, this))
            {
                return true;
            }

            return this.ResultType.Equals(other.ResultType)
                && this.Name == other.Name
                && this.Value == other.Value
                && this.Description == other.Description;
        }

        // https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 151;
                hash = hash * 443 + this.ResultType.GetHashCode();
                hash = hash * 443 + this.Name.GetHashCode();
                hash = hash * 443 + this.Value.GetHashCode();
                hash = hash * 443 + this.Description.GetHashCode();
                
                return hash;
            }
        }
        
        #endregion

        #region Operators

        public static bool operator ==(MemeCommand lhs, MemeCommand rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(MemeCommand lhs, MemeCommand rhs)
        {
            return !lhs.Equals(rhs);
        }

        #endregion
    }
}

using SteakBot.Core.Objects.Enums;
using System;

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

            return (object.ReferenceEquals(this.ResultType, other.ResultType)
                    || this.ResultType.Equals(other.ResultType))
                && (object.ReferenceEquals(this.Name, other.Name)
                    || this.Name != null
                    && this.Name.Equals(other.Name))
                && (object.ReferenceEquals(this.Value, other.Value)
                    || this.Value != null
                    && this.Value.Equals(other.Value))
                && (object.ReferenceEquals(this.Description, other.Description)
                    || this.Description != null
                    && this.Description.Equals(other.Description));
        }

        public override int GetHashCode()
        {
            return this.ResultType.GetHashCode()
                 ^ (this.Name != null ? this.Name.GetHashCode() : 0)
                 ^ (this.Value != null ? this.Value.GetHashCode() : 0)
                 ^ (this.Description != null ? this.Description.GetHashCode() : 0);
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

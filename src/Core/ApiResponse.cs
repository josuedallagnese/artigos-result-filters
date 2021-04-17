using System;
using System.Collections.Generic;
using System.Linq;

namespace ResultFilters.Core
{
    public class ApiResponse
    {
        public bool HasErrors => Errors.Count > 0;

        public List<string> Errors { get; set; }

        public ApiResponse()
        {
            Errors = new List<string>();
        }

        public ApiResponse Add(string error)
        {
            if (error is null)
                throw new ArgumentNullException(nameof(error));

            if (!Errors.Contains(error))
                Errors.Add(error);

            return this;
        }

        public ApiResponse Add(ApiResponse response)
        {
            if (response is null)
                throw new ArgumentNullException(nameof(response));


            foreach (var error in response.Errors)
                Add(error);

            return this;
        }

        public ApiResponse Remove(string error)
        {
            if (error is null)
                throw new ArgumentNullException(nameof(error));

            Errors.Remove(error);

            return this;
        }

        public bool HasError(string error)
        {
            return Errors.Contains(error, StringComparer.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ApiResponse response))
                return base.Equals(obj);

            if (obj == null)
                return false;

            if (response.Errors.Count != Errors.Count)
                return false;

            foreach (var error in response.Errors)
            {
                if (!HasError(error))
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return Errors.Aggregate(1, (current, error) => current * error.GetHashCode());
        }

        public static bool operator ==(ApiResponse left, ApiResponse right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
                return true;

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(ApiResponse left, ApiResponse right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            if (!HasErrors)
                return base.ToString();

            return string.Join(Environment.NewLine, Errors);
        }
    }
}

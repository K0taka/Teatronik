using Teatronik.Core.Common;

namespace Teatronik.Core.Models
{
    public class Component
    {
        public const int MAX_SERIALNUMBER_LENGTH = 50;

        public string SerialNumber { get; }
        public DateOnly AcquisitionDate { get; }
        public Guid ModelId { get; }
        public Guid? PropId { get; private set; }

        private Component(string serialNumber, DateOnly acquisitionDate, Guid modelId, Guid? propId = null)
        {
            SerialNumber = serialNumber;
            AcquisitionDate = acquisitionDate;
            ModelId = modelId;
            PropId = propId;
        }

        public static Result<Component> Create(
            string serialNumber, DateOnly acquisitionDate, Guid modelId, Guid? propId = null
            )
        {
            if (string.IsNullOrWhiteSpace(serialNumber))
                return Result<Component>.Fail("Serial number must be not empty");
            if (serialNumber.Length > MAX_SERIALNUMBER_LENGTH)
                return Result<Component>.Fail($"Serial number must be not greater than {MAX_SERIALNUMBER_LENGTH}");

            if (DateOnly.FromDateTime(DateTime.Now).CompareTo(acquisitionDate) < 0)
                return Result<Component>.Fail("Acquisition date must be not later than today");

            if (Guid.Empty.Equals(modelId))
                return Result<Component>.Fail("modelId must be not empty");

            if (propId != null && Guid.Empty.Equals(propId))
                return Result<Component>.Fail("propId must be not empty");

            return Result<Component>.Ok(new(serialNumber, acquisitionDate, modelId, propId));
        }

        public Result AssignToProp(Guid propId)
        {
            if (propId == Guid.Empty)
                return Result.Fail("Invalid Prop ID");

            PropId = propId;
            return Result.Ok();
        }
    }
}

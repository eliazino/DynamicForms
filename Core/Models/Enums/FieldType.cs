using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models.Enums {
    public enum FieldType {
        DATE = 1,
        DATE_AND_TIME = 2,
        TEXT = 3,
        NUMBER = 4,
        LONG_NUMBER = 6,
        DROP_DOWN = 5
    }

    public enum FieldBehaviour {
        UNIQUE = 1,
        AUTO_GENERATED_ID = 3,
        AUTO_GENERATED_DATE = 4,
        REQUIRED = 5,
        FILTERABLE_INPUT = 6,
        FILTERABLE_DROPDOWN = 7,
        FILTERABLE_DATE_RANGE = 8
    }
}

using System;
using Skyline.DataMiner.Analytics.GenericInterface;

[GQIMetaData(Name = "DoubleToDateTime")]
public class DoubleToDateTime : IGQIRowOperator, IGQIInputArguments, IGQIColumnOperator
{
    private readonly GQIColumnDropdownArgument _dateTimeColumnArg = new GQIColumnDropdownArgument("Double Column") { IsRequired = true, Types = new GQIColumnType[] { GQIColumnType.Double} };
    private GQIColumn<double> _doubleColumn;
    private GQIColumn<DateTime> _dateTimeColumn;

    public GQIArgument[] GetInputArguments()
    {
        return new GQIArgument[] { _dateTimeColumnArg };
    }

    public OnArgumentsProcessedOutputArgs OnArgumentsProcessed(OnArgumentsProcessedInputArgs args)
    {
        _doubleColumn = args.GetArgumentValue(_dateTimeColumnArg) as GQIColumn<double>;
        if (_doubleColumn != null)
            _dateTimeColumn = new GQIDateTimeColumn($"{_doubleColumn.Name} (as DateTime)");

        return new OnArgumentsProcessedOutputArgs();
    }

    public void HandleColumns(GQIEditableHeader header)
    {
        if (_dateTimeColumn != null)
            header.AddColumns(_dateTimeColumn);
    }

    public void HandleRow(GQIEditableRow row)
    {
        if (_dateTimeColumn == null || _doubleColumn == null)
            return;

        double date = row.GetValue<double>(_doubleColumn);
        row.SetValue(_dateTimeColumn, DateTime.FromOADate(date).ToUniversalTime());
    }
}
using Dapper;
using Npgsql;
using NpgsqlTypes;

namespace EVCP.Domain.Helpers;

public class PointTypeMapper : SqlMapper.TypeHandler<NetTopologySuite.Geometries.Point> {
    public override void SetValue(System.Data.IDbDataParameter parameter, NetTopologySuite.Geometries.Point value) {
        if (parameter is NpgsqlParameter npgsqlParameter) {
            npgsqlParameter.NpgsqlDbType = NpgsqlDbType.Geography;
            npgsqlParameter.NpgsqlValue = value;
        } else {
            throw new ArgumentException();
        }
    }

    public override NetTopologySuite.Geometries.Point Parse(object value) {
        if (value is NetTopologySuite.Geometries.Point geometry) {
            return geometry;
        } 

        throw new ArgumentException();
    }
}
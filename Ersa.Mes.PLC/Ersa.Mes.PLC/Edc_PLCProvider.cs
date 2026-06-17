using System;
using System.Collections.Generic;
using System.Linq;

namespace Ersa.Mes.PLC;

public class Edc_PLCProvider : Inf_PLCProvider
{
	public IEnumerable<Lazy<Inf_PLC, Inf_PLCMetadata>> Pro_fdcSpsProvider { get; set; }

	public Inf_PLC Fun_edcActiveSps()
	{
		if (!Edc_CommunicationHelper.Pro_strSpsType.Equals("PviServices") && !Edc_CommunicationHelper.Pro_strSpsType.Equals("M1Com"))
		{
			throw new Exception("Invalid Value...Edc_CommunicationHelper.Pro_strSpsType");
		}
		return Pro_fdcSpsProvider.First((Lazy<Inf_PLC, Inf_PLCMetadata> s) => s.Metadata.Pro_strSpsTyp == Edc_CommunicationHelper.Pro_strSpsType).Value;
	}
}

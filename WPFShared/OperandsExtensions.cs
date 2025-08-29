namespace WPFShared
{
	// Token: 0x0200004D RID: 77
	public static class OperandsExtensions
	{
		// Token: 0x06000215 RID: 533 RVA: 0x000091A8 File Offset: 0x000073A8
		public static bool IsMatch(this Operands op, int value, int known)
		{
			bool flag = false;
			if ((op & Operands.LessThan) == Operands.LessThan)
			{
				flag |= value < known;
			}
			if ((op & Operands.EqualTo) == Operands.EqualTo)
			{
				flag |= value == known;
			}
			if ((op & Operands.GreaterThan) == Operands.GreaterThan)
			{
				flag |= value >= known;
			}
			return flag;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x000091E4 File Offset: 0x000073E4
		public static bool IsMatch(this Operands op, string value, int known)
		{
			int num = 0;
			return int.TryParse(value, out num) && op.IsMatch(num, known);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00009208 File Offset: 0x00007408
		public static Operands FromString(string operandString)
		{
			Operands operands = Operands.None;
			if (!string.IsNullOrWhiteSpace(operandString))
			{
				for (int i = 0; i < operandString.Length; i++)
				{
					switch (operandString[i])
					{
					case '<':
						operands |= Operands.LessThan;
						break;
					case '=':
						operands |= Operands.EqualTo;
						break;
					case '>':
						operands |= Operands.GreaterThan;
						break;
					}
				}
			}
			return operands;
		}
	}
}

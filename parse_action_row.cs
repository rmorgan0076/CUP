namespace CUP
{
	using System;
	
	/// <summary>This class represents one row (corresponding to one machine state) of the 
	/// parse action table.
	/// </summary>
	public class parse_action_row
	{
		
		/*-----------------------------------------------------------*/
		/*--- Constructor(s) ----------------------------------------*/
		/*-----------------------------------------------------------*/
		
		/// <summary>Simple constructor.  Note: this should not be used until the number of
		/// terminals in the grammar has been established.
		/// </summary>
		public parse_action_row()
		{
			/* make sure the size is set */
			if (_size <= 0)
				_size = terminal.number();
			
			/* allocate the array */
			under_term = new parse_action[size()];
			
			/* set each element to an error action */
			 for (int i = 0; i < _size; i++)
				under_term[i] = new parse_action();
		}
		
		/*-----------------------------------------------------------*/
		/*--- (Access to) Static (Class) Variables ------------------*/
		/*-----------------------------------------------------------*/
		
		/// <summary>Number of columns (terminals) in every row. 
		/// </summary>
		protected static int _size = 0;
		
		/// <summary>Number of columns (terminals) in every row. 
		/// </summary>
		public static int size()
		{
			return _size;
		}
		
		/*. . . . . . . . . . . . . . . . . . . . . . . . . . . . . .*/
		
		/// <summary>Table of reduction counts (reused by compute_default()). 
		/// </summary>
		protected static int[] reduction_count = null;
		
		/*-----------------------------------------------------------*/
		/*--- (Access to) Instance Variables ------------------------*/
		/*-----------------------------------------------------------*/
		
		/// <summary>Actual action entries for the row. 
		/// </summary>
		public parse_action[] under_term;
		
		/*. . . . . . . . . . . . . . . . . . . . . . . . . . . . . .*/
		
		/// <summary>Default (reduce) action for this row.  -1 will represent default 
		/// of error. 
		/// </summary>
		public int default_reduce;
		
		/*-----------------------------------------------------------*/
		/*--- General Methods ---------------------------------------*/
		/*-----------------------------------------------------------*/
		
		/// <summary>Compute the default (reduce) action for this row and store it in 
		/// default_reduce.  In the case of non-zero default we will have the 
		/// effect of replacing all errors by that reduction.  This may cause 
		/// us to do erroneous reduces, but will never cause us to shift past 
		/// the point of the error and never cause an incorrect parse.  -1 will 
		/// be used to encode the fact that no reduction can be used as a 
		/// default (in which case error will be used).
		/// </summary>
		public virtual void  compute_default()
		{
			int i, prod, max_prod, max_red;
			
			/* if we haven't allocated the count table, do so now */
			if (reduction_count == null)
				reduction_count = new int[production.number()];
			
			/* clear the reduction count table and maximums */
			 for (i = 0; i < production.number(); i++)
				reduction_count[i] = 0;
			max_prod = - 1;
			max_red = 0;
			
			/* walk down the row and look at the reduces */
			 for (i = 0; i < size(); i++)
				if (under_term[i].kind() == parse_action.REDUCE)
				{
					/* count the reduce in the proper production slot and keep the 
					max up to date */
					prod = ((reduce_action) under_term[i]).reduce_with().index();
					reduction_count[prod]++;
					if (reduction_count[prod] > max_red)
					{
						max_red = reduction_count[prod];
						max_prod = prod;
					}
				}
			
			/* record the max as the default (or -1 for not found) */
			default_reduce = max_prod;
		}
		
		/*-----------------------------------------------------------*/
	}
}
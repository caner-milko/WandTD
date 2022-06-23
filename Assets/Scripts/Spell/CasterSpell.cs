namespace wtd.spell
{
	public class CasterSpell
	{
		public Spell Spell { get; private set; }
		public bool IsActive { get; private set; }
		public ISpellCaster Owner { get; private set; }

		public CasterSpell(Spell spell, ISpellCaster owner)
		{
			this.Spell = spell;
			this.Owner = owner;
			this.IsActive = this.Spell is ActiveSpell;
		}

		public void SetSpell(Spell newSpell)
		{
			this.Spell = newSpell;
			this.IsActive = this.Spell is ActiveSpell;
		}

		public CasterSpell CloneToOwner(ISpellCaster newOwner)
		{
			return new CasterSpell(Spell, newOwner);
		}

	}
}

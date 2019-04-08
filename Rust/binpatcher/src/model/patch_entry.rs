use super::*;

#[derive(Debug, Clone, PartialEq, Eq)]
pub enum MatchBy
{
    Address,
    FirstMatch,
    SingleMatch,
    EveryMatch
}

#[derive(Debug, Clone, PartialEq, Eq)]
pub struct PatchEntry {
    address: usize,
    match_by: MatchBy,
    old_data: Pattern,
    new_data: Pattern
}

impl PatchEntry {
    pub fn new(address: usize, match_by: MatchBy, old_data: Pattern, new_data: Pattern) -> Self {
        PatchEntry{ address, match_by, old_data, new_data }
    }

    pub fn address(&self) -> usize {
        self.address
    }

    pub fn match_by(&self) -> &MatchBy {
        &self.match_by
    }

    pub fn old_data(&self) -> &Pattern {
        &self.old_data
    }

    pub fn new_data(&self) -> &Pattern {
        &self.new_data
    }
}

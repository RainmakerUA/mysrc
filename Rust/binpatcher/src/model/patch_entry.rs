use super::Pattern;

#[derive(Debug, Clone, PartialEq, Eq)]
pub enum MatchBy {
    Address(usize),
    FirstMatch,
    SingleMatch,
    EveryMatch,
}

#[derive(Debug, Clone, PartialEq, Eq)]
pub struct PatchEntry {
    match_by: MatchBy,
    old_data: Pattern,
    new_data: Pattern,
}

impl PatchEntry {
    pub fn new(match_by: MatchBy, old_data: Pattern, new_data: Pattern) -> Self {
        PatchEntry {
            match_by,
            old_data,
            new_data,
        }
    }

    pub fn address(&self) -> Option<usize> {
        match self.match_by {
            MatchBy::Address(addr) => Some(addr),
            _ => None,
        }
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

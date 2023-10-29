use std::fmt;

#[derive(Debug, Clone, PartialEq, Eq)]
pub struct ParseBytePartError {
    kind: BytePartErrorKind,
}

#[derive(Debug, Clone, PartialEq, Eq)]
pub(super) enum BytePartErrorKind {
    InvalidChar(u8),
    WrongLength,
}

#[derive(Debug, Clone, PartialEq, Eq)]
pub struct PatternError {
    kind: PatternErrorKind,
}

#[derive(Debug, Clone, PartialEq, Eq)]
pub(super) enum PatternErrorKind {
    SizeMismatch,
}

impl ParseBytePartError {
    pub(super) fn from_kind(kind: BytePartErrorKind) -> Self {
        ParseBytePartError { kind }
    }
    pub(super) fn description(&self) -> String {
        match self.kind {
            BytePartErrorKind::InvalidChar(ch) => format!("Invalid character '{}'!", ch as char),
            BytePartErrorKind::WrongLength => String::from("Wrong length!"),
        }
    }
}

impl fmt::Display for ParseBytePartError {
    fn fmt(&self, f: &mut fmt::Formatter) -> fmt::Result {
        self.description().fmt(f)
    }
}

impl PatternError {
    pub(super) fn from_kind(kind: PatternErrorKind) -> Self {
        PatternError { kind }
    }
}

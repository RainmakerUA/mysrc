mod bytepart;
mod errors;
mod patch;
mod patch_entry;
mod pattern;

pub use self::bytepart::BytePart;
pub use self::errors::*;
pub use self::patch::Patch;
pub use self::patch_entry::{MatchBy, PatchEntry};
pub use self::pattern::Pattern;

#[cfg(test)]
mod test {
    // TODO: Is it needed?
    //use super::*;
}

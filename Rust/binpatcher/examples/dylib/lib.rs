#![cfg(windows)]

extern crate winapi;

use winapi::{
    shared::minwindef::{self, BOOL, DWORD, HINSTANCE, LPVOID},
    um::winnt::{DLL_PROCESS_ATTACH, DLL_PROCESS_DETACH},
};

#[no_mangle]
#[allow(non_snake_case, unused_variables)]
pub extern "system" fn DllMain(hmodule: HINSTANCE, reason: DWORD, reserved: LPVOID) -> BOOL {
    match reason {
        DLL_PROCESS_ATTACH => lib_init(),
        DLL_PROCESS_DETACH => lib_deinit(),
        _ => (),
    }
    minwindef::TRUE
}

#[no_mangle]
#[allow(non_snake_case, unused_variables)]
pub extern "system" fn TestRust(x: i32, y: i32) -> i32 {
    x + y
}

fn lib_init() {
    println!("dylib initialized!")
}

fn lib_deinit() {
    println!("dylib deinitialized!")
}

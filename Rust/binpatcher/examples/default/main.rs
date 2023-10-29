extern crate binpatcher;

use std::fs::File;
use std::io::Read;
use std::time::Instant;

use binpatcher::{
    matching::*,
    model::{BytePart, Pattern},
};

fn main() {
    let bp1 = BytePart::full(0xF2);
    let bp2 = BytePart::low(0xF);

    println!("{}", bp1);
    println!("{:?}", bp2);

    println!("Any byte: {}", BytePart::any());
    println!("Low byte part: {}", BytePart::low(0xC));
    println!("High byte part: {}", BytePart::high(0xA));
    println!("Full byte: {}", BytePart::full(0x4A));

    println!("BytePart: {:?}", BytePart::low(0xFF));

    println!("BytePart: {}", parse_bytepart("?1"));
    println!("BytePart: {}", parse_bytepart("??"));
    println!("BytePart: {}", parse_bytepart("F1"));
    println!("BytePart: {}", parse_bytepart("C?"));
    //println!("BytePart: {}", parse_bytepart("X1")); // panic!

    let p = Pattern::parse("7E ??????04 25 17 6A 58").unwrap();
    let b = &mut [0x7E, 0x23, 0x65, 0x99, 0x04, 0x25, 0x17, 0x6A, 0x58];
    println!("Is Match: {:?}", p.is_match(b));

    let patch = Pattern::parse("?? ???????? 2B ?? ?? ??").unwrap();
    match patch.apply(b) {
        Ok(_) => println!("Applied: {:?}", b),
        Err(error) => println!("{:?}", error),
    }

    match Pattern::parse("?? ???????? 2B ?? ? ??") {
        Ok(p) => println!("{:?}", p),
        Err(e) => println!("Error: {}", e),
    }

    let p = Pattern::parse("5? A2 ?? ?8").unwrap();
    let b = &[
        0x33, 0x5F, 0xA2, 0xCC, 0xD8, 0x23, 0x00, 0x54, 0xA2, 0xDD, 0x28, 0x30,
    ];

    for pos in matches_from_slice(&p, b, None) {
        println!("Match found @ 0x{:02X}", pos)
    }

    let use_buff_reader = true;
    let mut f: File = File::open(r"d:\Install\M42 Prereqs\REDISTB.zip").expect("Cannot open file!");
    let f_size = f.metadata().expect("Cannot get file metadata!").len() as usize;

    let p = Pattern::parse("5? A2 ?? ?8 FF E?").unwrap();
    let start = Instant::now();

    if use_buff_reader {
        println!("Reading a file (buffered)...");
        for pos in matches_from_reader(&p, f, 256 * 1024 * 1024) {
            println!(
                "Match found @ 0x{:08X} ({:02.2}% after {:?})",
                pos,
                100 * pos / f_size,
                start.elapsed()
            );
        }
    } else {
        let mut v = Vec::with_capacity(f_size + 1);
        let _ = f.read_to_end(&mut v).expect("Cannot read file!");
        for pos in matches_from_slice(&p, &v, None) {
            println!(
                "Match found @ 0x{:02X} : {:02}% | Elapsed: {:?}",
                pos,
                100 * pos / f_size,
                start.elapsed()
            );
        }
    }

    let elapsed_seconds = start.elapsed().as_secs_f64();
    let speed_text = get_data_speed_text(f_size, elapsed_seconds);
    println!(
        "Scan finished after {:.2} s. Average speed: {}",
        elapsed_seconds, speed_text
    );
}

fn parse_bytepart(s: &str) -> BytePart {
    match s.parse::<BytePart>() {
        Ok(bp) => bp,
        Err(err) => panic!("Cannot parse BytePart '{}': {}", s, err),
    }
}

fn get_data_speed_text(len: usize, elapsed: f64) -> String {
    let mut speed = (len as f64) / elapsed;
    let mut prefix_num = 0u8;

    while speed > 1024f64 && prefix_num <= 3 {
        speed /= 1024f64;
        prefix_num += 1;
    }

    let prefix = match prefix_num {
        0 => "",
        1 => "ki",
        2 => "Mi",
        3 => "Gi",
        _ => "?i",
    };

    format!("{:.3} {}B/s", speed, prefix)
}

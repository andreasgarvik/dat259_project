type Person =
    { Name: string
      Birthday: System.DateTime }

let age person =
    let daysDiff =
        System
            .DateTime
            .Today
            .Subtract(
                person.Birthday
            )
            .Days

    daysDiff / 365

let p =
    { Name = "Andreas"
      Birthday = System.DateTime(1997, 3, 13) }

printfn $"{age p}"

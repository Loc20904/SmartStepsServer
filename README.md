# SmartStepsServer
Source code server for SmartSteps application.

## Seeded situation content

The SmartSteps lesson catalog is configured in `SmartStepsDbContext` with EF
Core `HasData`. The generated migration creates 3 islands and 9 published
situations from the SmartSteps content document, including steps, flashcards,
skills, and parent review notes. Startup does not run a custom seeder.

Published lessons are exposed to the app through:

```http
GET /api/situations
GET /api/situations/{id}
```

## Private Supabase Storage media

Use the backend to create short-lived signed URLs for private Storage files. Do not put the Supabase `service_role` key in the mobile app.

Set these values on the server through environment variables:

```powershell
$env:SupabaseStorage__Url="https://your-project-ref.supabase.co"
$env:SupabaseStorage__ServiceRoleKey="your-service-role-key"
$env:SupabaseStorage__Bucket="smartsteps-media"
$env:SupabaseStorage__SignedUrlExpiresInSeconds="300"
```

Call the API with a Supabase Auth access token:

```http
POST /api/media/signed-url
Authorization: Bearer <supabase-access-token>
Content-Type: application/json

{
  "stepId": 1
}
```

The endpoint only signs media from a published `SituationStep`. Store `SituationStep.MediaUrl` as either the object path inside the bucket, for example `videos/lesson-1.mp4`, or a Supabase Storage URL for the configured bucket.

## Premium subscriptions and payOS

Premium access is stored on the server in `PremiumSubscription`. Expired
subscriptions are marked as `Expired` whenever premium status is requested, so
locked lessons automatically become locked again after the subscription end
date.

Apply the premium schema migration:

```powershell
dotnet ef database update --project SmartStepsServer/SmartStepsServer.csproj --startup-project SmartStepsServer/SmartStepsServer.csproj
```

Configure payOS from the payment channel on `https://my.payos.vn`:

```powershell
$env:PayOS__ClientId="your-payos-client-id"
$env:PayOS__ApiKey="your-payos-api-key"
$env:PayOS__ChecksumKey="your-payos-checksum-key"
$env:PayOS__ReturnUrl="http://localhost:3000/learning?premiumPayment=success"
$env:PayOS__CancelUrl="http://localhost:3000/learning?premiumPayment=cancel"
```

For production, set the webhook URL in payOS to:

```http
https://your-domain.com/api/premium/payos/webhook
```

For local webhook testing, expose the API through a tunnel such as ngrok and use
the tunnel URL. You can also confirm the webhook URL through the backend:

```http
POST /api/premium/payos/confirm-webhook
Content-Type: application/json

{
  "webhookUrl": "https://your-domain.com/api/premium/payos/webhook"
}
```

The MVP test code is `PREMIUM`. It activates one month of premium access and can
only be redeemed once per account email.

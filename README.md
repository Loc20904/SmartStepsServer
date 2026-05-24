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
